// Made by lovely Waveform
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace HutongGames.PlayMaker.Actions
{

	[ActionCategory("Post Processing V2")]
	public class ActiveColorGradingTrackballs : FsmStateAction
	{
        [ActionSection("Profile")]
        [ObjectType(typeof(PostProcessProfile))]
        public FsmObject Profile;
        [ObjectType(typeof(PostProcessVolume))]
        public FsmObject Volume;

        //[ActionSection("Enable")]
        public FsmBool ActiveEnable;
        //public FsmBool EnableValue;

        //[ActionSection("Trackballs Lift")]
        public FsmBool ActiveLift;
        //public FsmBool LiftColor;

        //[ActionSection("Trackballs Gamma")]
        public FsmBool ActiveGamma;
        //public FsmBool GammaColor;

        //[ActionSection("Trackballs Gain")]
        public FsmBool ActiveGain;
        //public FsmBool GainColor;


        [ActionSection(" ")]
        public bool everyFrame;


        private PostProcessProfile convert;
        private PostProcessVolume convert2;

        public override void Reset()
        {
            Profile = null;
            convert = null;
            convert2 = null;

            ActiveEnable = new FsmBool { UseVariable = true };
            ActiveLift = new FsmBool { UseVariable = true };
            ActiveGain = new FsmBool { UseVariable = true };
            ActiveGamma = new FsmBool { UseVariable = true };

            everyFrame = false;

        }
        public override void OnEnter()
		{
            ggop();

            if(!everyFrame)
            {
                Finish();
            }

        }

        public override void OnUpdate()
        {
            ggop();
        }
        private void ggop()
        {
            if (Profile.Value != null)
            {
                convert = (PostProcessProfile)Profile.Value;
            }
            else if (Volume.Value != null)
            {
                convert2 = (PostProcessVolume)Volume.Value;
                convert = convert2.profile;
            }
            if (convert == null)
            {
                return;
            }
            else
            {
                convert.TryGetSettings(out ColorGrading colorGrading);

                if (!ActiveEnable.IsNone)
                    colorGrading.active = ActiveEnable.Value;
                if (!ActiveLift.IsNone)
                    colorGrading.lift.overrideState = ActiveLift.Value;
                if (!ActiveGamma.IsNone)
                    colorGrading.gamma.overrideState = ActiveGamma.Value;
                if (!ActiveGain.IsNone)
                    colorGrading.gain.overrideState = ActiveGain.Value;



            }



        }
	}

}
