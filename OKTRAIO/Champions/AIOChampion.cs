namespace OKTRAIO.Champions
{
    internal abstract class AIOChampion
    {
        public abstract void Init(); //100% necessary, abstract
        //Init only needs to declare abilities and necessary events for the specific champion
        public abstract void Combo(); //100% necessary, abstract

        public virtual void Harass()
        {
        }

        public virtual void Laneclear()
        {
        }

        public virtual void Jungleclear()
        {
        }

        public virtual void Flee()
        {
        }

        public virtual void LastHit()
        {
        }
    }
}