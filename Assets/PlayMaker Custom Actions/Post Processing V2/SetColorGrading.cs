// Made by lovely Waveform
using UnityEngine.Rendering.PostProcessing;

namespace HutongGames.PlayMaker.Actions
{

	[ActionCategory("Post Processing V2")]
	public class SetColorGrading : FsmStateAction
	{
        [ActionSection("Profile")]
        [ObjectType(typeof(PostProcessProfile))]
        public FsmObject Profile;
        [ObjectType(typeof(PostProcessVolume))]
        public FsmObject Volume;

        [ActionSection("Enable")]
        public FsmBool SetEnable;
        public FsmBool EnableValue;

        [ActionSection("Mode")]
        public FsmBool SetMode;
        [ObjectType(typeof(GradingMode))]
        public FsmEnum ModeValue;

        [ActionSection("Temperature")]
        public FsmBool SetTemperature;
        [HasFloatSlider(-100,100)]
        public FsmFloat TemperatureValue;

        [ActionSection("Tint")]
        public FsmBool SetTint;
        [HasFloatSlider(-100, 100)]
        public FsmFloat TintValue;

        [ActionSection("PostExposureEV")]
        public FsmBool SetPostExposureEV;
        public FsmFloat PostExposureEVValue;

        [ActionSection("ColorFilter")]
        public FsmBool SetColorFilter;
        public FsmColor ColorFilterValue;

        [ActionSection("HueShift")]
        public FsmBool SetHueShift;
        [HasFloatSlider(-180, 180)]
        public FsmFloat HueShiftValue;

        [ActionSection("Saturation")]
        public FsmBool SetSaturation;
        [HasFloatSlider(-100, 100)]
        public FsmFloat SaturationValue;

        [ActionSection("Contrast")]
        public FsmBool SetContrast;
        [HasFloatSlider(-100, 100)]
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

            SetEnable = false;
            EnableValue = false;

            SetMode= false;
            ModeValue = GradingMode.HighDefinitionRange;

            SetTemperature = false;
            TemperatureValue = 0;

            SetTint = false;
            TintValue = 0;

            SetPostExposureEV = false;
            PostExposureEVValue = 0;

            SetColorFilter = false;
            ColorFilterValue = default;

            SetHueShift = false;
            HueShiftValue = 0;

            SetSaturation = false;
            SaturationValue = 0;

            SetContrast = false;
            ContrastValue = 0;


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

                if (SetEnable.Value)
                    colorGrading.enabled.value = EnableValue.Value;
                if (SetMode.Value)
                    colorGrading.gradingMode.value = (GradingMode)ModeValue.Value;
                if (SetTemperature.Value)
                    colorGrading.temperature.value = TemperatureValue.Value;
                if (SetTint.Value)
                    colorGrading.tint.value = TintValue.Value;
                if (SetPostExposureEV.Value)
                    colorGrading.postExposure.value = PostExposureEVValue.Value;
                if (SetColorFilter.Value)
                    colorGrading.colorFilter.value = ColorFilterValue.Value;
                if (SetHueShift.Value)
                    colorGrading.hueShift.value = HueShiftValue.Value;
                if (SetSaturation.Value)
                    colorGrading.saturation.value = SaturationValue.Value;
                if (SetContrast.Value)
                    colorGrading.contrast.value = ContrastValue.Value;

            }



        }
	}

}
