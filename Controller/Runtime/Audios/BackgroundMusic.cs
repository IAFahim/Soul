using Alchemy.Inspector;
using Pancake.Sound;
using UnityEngine;

namespace Soul.Controller.Runtime
{
    public class BackgroundMusic : MonoBehaviour
    {
        [HorizontalLine, SerializeField, AudioPickup] private AudioId bgm;

        private void Start()
        {
            AudioManager.MusicVolume = 1;
            AudioManager.SfxVolume = 1;
            
            bgm.Play();
        }
    }
}