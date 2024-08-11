using Pancake.Common;
using UnityEngine;

namespace Soul.Model.Runtime.Game
{
    public class FirstOpenStartup : MonoBehaviour
    {
        private void Start()
        {
            if (!UserData.GetFirstOpen())
            {
                UserData.SetFirstOpen(true);

                string userId = UserData.UserId;
                Data.Save(Constant.User.KEY_ID, userId);
            }
        }
    }
}
