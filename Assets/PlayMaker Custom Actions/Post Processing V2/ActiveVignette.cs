// Made by lovely Waveform
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace HutongGames.PlayMaker.Actions
{

	[ActionCategory("Post Processing V2")]
	public class ActiveVignette : FsmStateAction
	{
        [ActionSection("Profile")]
        [ObjectType(typeof(PostProcessProfile))]
        public FsmObject Profile;
        [ObjectType(typeof(PostProcessVolume))]
        public FsmObject Volume;

        //[ActionSection("Enable")]
        public FsmBool ActiveEnable;
        //public FsmBool EnableValue;

        //[ActionSection("Mode")]
        public FsmBool ActiveMode;
        //public FsmBool ModeValue;

        //[ActionSection("Color")]
        public FsmBool ActiveColor;
        //public FsmBool ColorValue;

        //[ActionSection("Center")]
        public FsmBool ActiveCenter;
        //public FsmBool CenterValue;

        //[ActionSection("Intensity")]
        public FsmBool ActiveIntensity;
        //public FsmBool IntensityValue;

        //[ActionSection("Smoothness")]
        public FsmBool ActiveSmoothness;
        //public FsmBool SmoothnessValue;

        //[ActionSection("Roundness")]
        public FsmBool ActiveRoundness;
        //public FsmBool RoundnessValue;

        //[ActionSection("Rounded")]
        public FsmBool ActiveRounded;
        //public FsmBool RoundedValue;

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

            ActiveMode = new FsmBool { UseVariable = true };

            ActiveColor = new FsmBool { UseVariable = true };

            ActiveCenter = new FsmBool { UseVariable = true };

            ActiveIntensity = new FsmBool { UseVariable = true };

            ActiveSmoothness = new FsmBool { UseVariable = true };

            ActiveRoundness = new FsmBool { UseVariable = true };

            ActiveRounded = new FsmBool { UseVariable = true };

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
                convert.TryGetSettings(out Vignette vignette);

                if (!ActiveEnable.IsNone)
                    vignette.active = ActiveEnable.Value;
                if (!ActiveMode.IsNone)
                    vignette.mode.overrideState = ActiveMode.Value;
                if (!ActiveColor.IsNone)
                    vignette.color.overrideState = ActiveColor.Value;
                if (!ActiveCenter.IsNone)
                    vignette.center.overrideState = ActiveCenter.Value;
                if (!ActiveIntensity.IsNone)
                    vignette.intensity.overrideState = ActiveIntensity.Value;
                if (!ActiveSmoothness.IsNone)
                    vignette.smoothness.overrideState = ActiveSmoothness.Value;
                if (!ActiveRoundness.IsNone)
                    vignette.roundness.overrideState = ActiveRoundness.Value;
                if (!ActiveRounded.IsNone)
                    vignette.rounded.overrideState = ActiveRounded.Value;

            }



        }
	}

}
