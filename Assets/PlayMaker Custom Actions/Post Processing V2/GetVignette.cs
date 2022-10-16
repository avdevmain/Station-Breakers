// Made by lovely Waveform
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace HutongGames.PlayMaker.Actions
{

	[ActionCategory("Post Processing V2")]
	public class GetVignette : FsmStateAction
	{
        [ActionSection("Profile")]
        [ObjectType(typeof(PostProcessProfile))]
        public FsmObject Profile;
        [ObjectType(typeof(PostProcessVolume))]
        public FsmObject Volume;
        [UIHint(UIHint.Variable)]
        [ObjectType(typeof(PostProcessVolume))]
        public FsmObject VolumeProfile;

        //[ActionSection("Enable")]
        //public FsmBool GetEnable;
        [UIHint(UIHint.Variable)]
        public FsmBool EnableValue;

        //[ActionSection("Mode")]
        //public FsmBool GetMode;
        [UIHint(UIHint.Variable)]
        [ObjectType(typeof(VignetteMode))]
        public FsmEnum ModeValue;

        //[ActionSection("Color")]
        //public FsmBool GetColor;
        [UIHint(UIHint.Variable)]
        public FsmColor ColorValue;

        //[ActionSection("Center")]
        //public FsmBool GetCenter;
        [UIHint(UIHint.Variable)]
        public FsmVector2 CenterValue;

        //[ActionSection("Intensity")]
        //public FsmBool GetIntensity;
        [UIHint(UIHint.Variable)]
        public FsmFloat IntensityValue;

        //[ActionSection("Smoothness")]
        //public FsmBool GetSmoothness;
        [UIHint(UIHint.Variable)]
        public FsmFloat SmoothnessValue;

        //[ActionSection("Roundness")]
        //public FsmBool GetRoundness;
        [UIHint(UIHint.Variable)]
        public FsmFloat RoundnessValue;

        //[ActionSection("Rounded")]
        //public FsmBool GetRounded;
        [UIHint(UIHint.Variable)]
        public FsmBool RoundedValue;

        [ActionSection(" ")]
        public bool everyFrame;


        private PostProcessProfile convert;
        private PostProcessVolume convert2;

        public override void Reset()
        {
            Profile = null;
            convert = null;
            convert2 = null;
            /*
            GetEnable = false;

            GetMode = false;

            GetColor = false;

            GetCenter = false;

            GetIntensity = false;

            GetSmoothness = false;

            GetRoundness = false;

            GetRounded = false;
            */
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
                VolumeProfile.Value = convert;
            }
            if (convert == null)
            {
                return;
            }
            else
            {
                convert.TryGetSettings(out Vignette vignette);

                if (!EnableValue.IsNone)
                    EnableValue.Value=vignette.enabled.value;
                if (!ModeValue.IsNone)
                    ModeValue.Value=vignette.mode.value;
                if (!ColorValue.IsNone)
                    ColorValue.Value=vignette.color.value;
                if (!CenterValue.IsNone)
                    CenterValue.Value=vignette.center.value;
                if (!IntensityValue.IsNone)
                    IntensityValue.Value=vignette.intensity.value;
                if (!SmoothnessValue.IsNone)
                    SmoothnessValue.Value=vignette.smoothness.value;
                if (!RoundnessValue.IsNone)
                    RoundnessValue.Value=vignette.roundness.value;
                if (!RoundedValue.IsNone)
                    RoundedValue.Value=vignette.rounded.value;

            }



        }
	}

}
