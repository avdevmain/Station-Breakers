// Made by lovely Waveform
using UnityEngine.Rendering.PostProcessing;

namespace HutongGames.PlayMaker.Actions
{

	[ActionCategory("Post Processing V2")]
	public class GetLensDistortion : FsmStateAction
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

        //[ActionSection("Intensity")]
        //public FsmBool GetIntensity;
        [UIHint(UIHint.Variable)]
        public FsmFloat IntensityValue;

        //[ActionSection("X Multiplier")]
        //public FsmBool GetXMultiplier;
        [UIHint(UIHint.Variable)]
        public FsmFloat XMultiplierValue;

        //[ActionSection("Y Multiplier")]
        //public FsmBool GetYMultiplier;
        [UIHint(UIHint.Variable)]
        public FsmFloat YMultiplierValue;

        //[ActionSection("CenterX")]
        //public FsmBool GetCenterX;
        [UIHint(UIHint.Variable)]
        public FsmFloat CenterXValue;

        //[ActionSection("CenterY")]
        //public FsmBool GetCenterY;
        [UIHint(UIHint.Variable)]
        public FsmFloat CenterYValue;

        //[ActionSection("Scale")]
        //public FsmBool GetScale;
        [UIHint(UIHint.Variable)]
        public FsmFloat ScaleValue;

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
            EnableValue = false;

            GetIntensity = false;

            GetXMultiplier = false;

            GetYMultiplier = false;

            GetCenterX = false;

            GetCenterY = false;

            GetScale = false;
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
                convert.TryGetSettings(out LensDistortion lensDistortion);

                if (!EnableValue.IsNone)
                    EnableValue.Value=lensDistortion.enabled.value;
                if (!IntensityValue.IsNone)
                    IntensityValue.Value=lensDistortion.intensity.value;
                if (!XMultiplierValue.IsNone)
                    XMultiplierValue.Value=lensDistortion.intensityX.value;
                if (!YMultiplierValue.IsNone)
                    YMultiplierValue.Value=lensDistortion.intensityY.value;
                if (!CenterXValue.IsNone)
                    CenterXValue.Value=lensDistortion.centerX.value;
                if (!CenterYValue.IsNone)
                    CenterYValue.Value=lensDistortion.centerY.value;
                if (!ScaleValue.IsNone)
                    ScaleValue.Value=lensDistortion.scale.value;


            }



        }
	}

}
