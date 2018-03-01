using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace SqlieTest_x
{
    public class AudioService
    {
        //private IAudio awx;

        public void Play() {
            DependencyService.Get<IAudio>().PlayAudioFile("beep.mp3");

        }
    }
}
