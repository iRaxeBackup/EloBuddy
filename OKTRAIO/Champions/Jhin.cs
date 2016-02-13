using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Notifications;
using OKTRAIO.Menu_Settings;

namespace OKTRAIO.Champions
{
    internal class Jhin : AIOChampion
    {

        private static Spell.Targeted Q;
        private static Spell.Skillshot W, E, R;

        public override void Init()
        {
            Q = new Spell.Targeted(SpellSlot.Q, 550);
            W = new Spell.Skillshot(SpellSlot.W, 2500, SkillShotType.Linear, 750, 5000, 40);
            E = new Spell.Skillshot(SpellSlot.E, 2000, SkillShotType.Circular, 230, 1600, 120);
            R = new Spell.Skillshot(SpellSlot.R, 3500, SkillShotType.Linear, 210, 5000, 80);



            #region Menu

            //Combo Menu
            MainMenu.ComboKeys(defaultR: false);
            MainMenu.Combo.AddGroupLabel("Combo Settings", "combo.settings.label", true);
            MainMenu.Combo.AddLabel("Q Settings", 25, "combo.q.settings.label", true);
            var qMode = new List<string> { "Use on Enemy", "Try 100% Damage" };
            MainMenu.Combo.AddComboBox("combo.q.mode", "Q Mode on Combo", qMode, 0, true);

            //LaneClear
            MainMenu.LaneKeys(true, true, true,true,false,false,false,false);
            MainMenu.Lane.AddGroupLabel("Lane Clear Settings", "lane.settings.label", true);
            MainMenu.Lane.AddLabel("Q Settings",25,"lane.q.settings.label",true);
            MainMenu.Lane.AddSlider("lane.q.minions", "Use Q only if Kill plus than:", 1, 1, 3, true);
            #endregion

            Value.Init();
            Notifications.Show(new SimpleNotification("One Key to Report","Jhin loaded!"));
        }

        public static float GetDamage(SpellSlot slot, Obj_AI_Base enemy)
        {
            if (slot == SpellSlot.Q)
            {
                {
                    var damage = new float[] {60, 85, 110, 135, 160}[Q.Level];
                    var damageMul = new float[] {0.3f, 0.35f, 0.4f, 0.45f, 0.5f}[Q.Level];
                    return Player.Instance.CalculateDamageOnUnit(enemy, DamageType.Physical,
                        damage + damageMul*Player.Instance.TotalAttackDamage + 0.6f*Player.Instance.TotalMagicalDamage);
                }
            }
            else if (slot == SpellSlot.W)
            {
                {
                    var damage = new float[] {50, 85, 120, 155, 190}[W.Level];
                    if (enemy.IsMinion)
                    {
                        return Player.Instance.CalculateDamageOnUnit(enemy, DamageType.Physical,
                            damage*0.6f + Player.Instance.TotalAttackDamage*0.455f);
                    }
                    return Player.Instance.CalculateDamageOnUnit(enemy, DamageType.Physical,
                        damage + Player.Instance.TotalAttackDamage*0.7f);
                }
            }
            else if (slot == SpellSlot.E)
            {
                {
                    var damage = new float[] {20, 80, 140, 200, 260}[E.Level];
                    return Player.Instance.CalculateDamageOnUnit(enemy, DamageType.Physical,
                        damage + 1.2f*Player.Instance.TotalAttackDamage + Player.Instance.TotalMagicalDamage);
                }
            }
            else if (slot == SpellSlot.R)
            {
                {
                    var damage = new float[] {50, 125, 200}[R.Level];
                    return Player.Instance.CalculateDamageOnUnit(enemy, DamageType.Physical,
                        damage + 0.25f*Player.Instance.TotalAttackDamage);
                }
            }


            return 0;
        }

        public override void Combo()
        {
            throw new NotImplementedException();
        }

        public override void Laneclear()
        {
            if (!Player.Instance.IsValid || Player.Instance.IsRecalling()) return;

            if (Q.IsReady() && Value.Use("lane.q"))
            {
                var targetCount = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, Player.Instance.Position, Q.Range).Where(m => GetDamage(SpellSlot.Q, m) > m.Health);
                //var  = targetCount as IList<Obj_AI_Minion> ?? targetCount.ToList();
                //if(.Count() > Value.Get("lane.q.minions")|| !.Any()) return;
                //var target = .FirstOrDefault();
               // if (target == null) return;
                //Q.Cast(target);
            }
        }
    }
}
