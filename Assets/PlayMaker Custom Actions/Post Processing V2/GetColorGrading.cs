// Made by lovely Waveform
using UnityEngine.Rendering.PostProcessing;

namespace HutongGames.PlayMaker.Actions
{

	[ActionCategory("Post Processing V2")]
	public class GetColorGrading : FsmStateAction
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
        [ObjectType(typeof(GradingMode))]
        public FsmEnum ModeValue;

        //[ActionSection("Temperature")]
        //public FsmBool GetTemperature;
        [UIHint(UIHint.Variable)]
        public FsmFloat TemperatureValue;

        //[ActionSection("Tint")]
        //public FsmBool GetTint;
        [UIHint(UIHint.Variable)]
        public FsmFloat TintValue;

        //[ActionSection("PostExposureEV")]
        //public FsmBool GetPostExposureEV;
        [UIHint(UIHint.Variable)]
        public FsmFloat PostExposureEVValue;

        //[ActionSection("ColorFilter")]
        //public FsmBool GetColorFilter;
        [UIHint(UIHint.Variable)]
        public FsmColor ColorFilterValue;

        //[ActionSection("HueShift")]
        //public FsmBool GetHueShift;
        [UIHint(UIHint.Variable)]
        public FsmFloat HueShiftValue;

        //[ActionSection("Saturation")]
        //public FsmBool GetSaturation;
        [UIHint(UIHint.Variable)]
        public FsmFloat SaturationValue;

        //[ActionSection("Contrast")]
        //public FsmBool GetContrast;
        [UIHint(UIHint.Variable)]
        public FsmFloat ContrastValue;


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

            GetMode= false;

            GetTemperature = false;

            GetTint = false;

            GetPostExposureEV = false;

            GetColorFilter = false;

            GetHueShift = false;

            GetSaturation = false;

            GetContrast = false;
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
                convert.TryGetSettings(out ColorGrading colorGrading);

                if (!EnableValue.IsNone)
                    EnableValue.Value = colorGrading.enabled.value;
                if (!ModeValue.IsNone)
                    ModeValue.Value = colorGrading.gradingMode.value;
                if (!TemperatureValue.IsNone)
                    TemperatureValue.Value = colorGrading.temperature.value;
                if (!TintValue.IsNone)
                    TintValue.Value = colorGrading.tint.value;
                if (!PostExposureEVValue.IsNone)
                    PostExposureEVValue.Value = colorGrading.postExposure.value;
                if (!ColorFilterValue.IsNone)
                    ColorFilterValue.Value = colorGrading.colorFilter.value;
                if (!HueShiftValue.IsNone)
                    HueShiftValue.Value = colorGrading.hueShift.value;
                if (!SaturationValue.IsNone)
                    SaturationValue.Value = colorGrading.saturation.value;
                if (!ContrastValue.IsNone)
                    ContrastValue.Value = colorGrading.contrast.value;

            }



        }
	}

}
