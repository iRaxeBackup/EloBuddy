using System;
using System.Drawing;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
using OKTRAIO.Menu_Settings;

namespace OKTRAIO.Utility
{
    public class DamageIndicator
    {
        public delegate float DamageToUnitDelegate(Obj_AI_Base minion);

        private static int _height;
        private static int _width;
        private static int _xOffset;
        private static int _yOffset;

        private static DamageToUnitDelegate _damageToUnit;

        private static bool EnemyEnabled
        {
            get
            {
                var enemyDmg = MainMenu.Draw.Get<CheckBox>("draw.enemyDmg");
                return enemyDmg == null ? false : enemyDmg.CurrentValue;
            }
        }

        private static Color EnemyColor
        {
            get { return MainMenu.Draw.GetColor("draw.color.enemyDmg"); }
        }

        private static bool JungleEnabled
        {
            get
            {
                var jungleDmg = MainMenu.Draw.Get<CheckBox>("draw.jungleDmg");
                return jungleDmg == null ? false : jungleDmg.CurrentValue;
            }
        }

        private static Color JungleColor
        {
            get { return MainMenu.Draw.GetColor("draw.color.jungleDmg"); }
        }

        public static DamageToUnitDelegate DamageToUnit
        {
            get { return _damageToUnit; }

            set
            {
                if (_damageToUnit == null)
                {
                    Drawing.OnEndScene += OnEndScene;
                }

                _damageToUnit = value;
            }
        }

        private static void OnEndScene(EventArgs args)
        {
            if (_damageToUnit == null) return;

            if (EnemyEnabled)
            {
                foreach (var hero in EntityManager.Heroes.Enemies
                    .Where(x => x.IsValidTarget()
                                && x.IsHPBarRendered))
                {
                    _height = 9;
                    _width = 104;
                    _xOffset = 2;
                    _yOffset = -6;

                    DrawLine(hero);
                }
            }

            if (JungleEnabled)
            {
                foreach (
                    var unit in
                        EntityManager.MinionsAndMonsters.GetJungleMonsters(Player.Instance.Position, 2000)
                            .Where(x => x.IsValidTarget()
                                        && x.IsHPBarRendered))
                {
                    if ((unit.Name.Contains("Blue") || unit.Name.Contains("Red")) && !unit.Name.Contains("Mini"))
                    {
                        _height = 9;
                        _width = 142;
                        _xOffset = -4;
                        _yOffset = -8;
                    }
                    else if (unit.Name.Contains("Dragon"))
                    {
                        _height = 10;
                        _width = 143;
                        _xOffset = -4;
                        _yOffset = -6;
                    }
                    else if (unit.Name.Contains("Baron"))
                    {
                        _height = 12;
                        _width = 191;
                        _xOffset = -29;
                        _yOffset = -7;
                    }
                    else if (unit.Name.Contains("Herald"))
                    {
                        _height = 10;
                        _width = 142;
                        _xOffset = -4;
                        _yOffset = -8;
                    }
                    else if ((unit.Name.Contains("Razorbeak") || unit.Name.Contains("Murkwolf")) &&
                             !unit.Name.Contains("Mini"))
                    {
                        _width = 74;
                        _height = 3;
                        _xOffset = 30;
                        _yOffset = -8;
                    }
                    else if (unit.Name.Contains("Krug") && !unit.Name.Contains("Mini"))
                    {
                        _width = 80;
                        _height = 3;
                        _xOffset = 27;
                        _yOffset = -8;
                    }
                    else if (unit.Name.Contains("Gromp"))
                    {
                        _width = 86;
                        _height = 3;
                        _xOffset = 24;
                        _yOffset = -7;
                    }
                    else if (unit.Name.Contains("Crab"))
                    {
                        _width = 61;
                        _height = 2;
                        _xOffset = 36;
                        _yOffset = 21;
                    }
                    else if (unit.Name.Contains("RedMini") || unit.Name.Contains("BlueMini") ||
                             unit.Name.Contains("RazorbeakMini"))
                    {
                        _height = 2;
                        _width = 49;
                        _xOffset = 42;
                        _yOffset = 6;
                    }
                    else if (unit.Name.Contains("KrugMini") || unit.Name.Contains("MurkwolfMini"))
                    {
                        _height = 2;
                        _width = 55;
                        _xOffset = 39;
                        _yOffset = 6;
                    }
                    else
                    {
                        continue;
                    }

                    DrawLine(unit);
                }
            }
        }

        private static void DrawLine(Obj_AI_Base unit)
        {
            var damage = _damageToUnit(unit);
            if (damage <= 0) return;

            var barPos = unit.HPBarPosition;

            //Get remaining HP after damage applied in percent and the current percent of health
            var percentHealthAfterDamage = Math.Max(0, unit.TotalShieldHealth() - damage)/
                                           (unit.MaxHealth + unit.AllShield + unit.AttackShield + unit.MagicShield);
            var currentHealthPercentage = unit.TotalShieldHealth()/
                                          (unit.MaxHealth + unit.AllShield + unit.AttackShield + unit.MagicShield);

            //Calculate start and end point of the bar indicator
            var startPoint = barPos.X + _xOffset + percentHealthAfterDamage*_width;
            var endPoint = barPos.X + _xOffset + currentHealthPercentage*_width;
            var yPos = barPos.Y + _yOffset;

            //Draw the line
            Drawing.DrawLine(startPoint, yPos, endPoint, yPos, _height, unit is AIHeroClient ? EnemyColor : JungleColor);
        }
    }
}