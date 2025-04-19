using System.Collections;

namespace DefaultNamespace
{
    public class DailyRewardManager : InitableManager
    {
        public override IEnumerator Init()
        {
            //TODO добавить проверку можно ли выдать награду 
            return base.Init();
        }
    }
}