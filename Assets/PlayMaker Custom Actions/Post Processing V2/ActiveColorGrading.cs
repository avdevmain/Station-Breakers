// Made by lovely Waveform
using UnityEngine.Rendering.PostProcessing;

namespace HutongGames.PlayMaker.Actions
{

	[ActionCategory("Post Processing V2")]
	public class ActiveColorGrading : FsmStateAction
	{
        [ActionSection("Profile")]
        [ObjectType(typeof(PostProcessProfile))]
        public FsmObject Profile;
        [ObjectType(typeof(PostProcessVolume))]
        public FsmObject Volume;

        //[ActionSection("Root")]
        public FsmBool ActiveRoot;
        //public FsmBool RootValue;

        //[ActionSection("Mode")]
        public FsmBool ActiveMode;
        //public FsmBool ModeValue;

        //[ActionSection("Temperature")]
        public FsmBool ActiveTemperature;
        //public FsmBool TemperatureValue;

        //[ActionSection("Tint")]
        public FsmBool ActiveTint;
        //public FsmBool TintValue;

        //[ActionSection("PostExposureEV")]
        public FsmBool ActivePostExposureEV;
        //public FsmBool PostExposureEVValue;

        //[ActionSection("ColorFilter")]
        public FsmBool ActiveColorFilter;
        //public FsmBool ColorFilterValue;

        //[ActionSection("HueShift")]
        public FsmBool ActiveHueShift;
        //public FsmBool HueShiftValue;

        //[ActionSection("Saturation")]
        public FsmBool ActiveSaturation;
        //public FsmBool SaturationValue;

        //[ActionSection("Contrast")]
        public FsmBool ActiveContrast;
        //public FsmBool ContrastValue;


        [ActionSection(" ")]
        public bool everyFrame;


        private PostProcessProfile convert;
        private PostProcessVolume convert2;

        public override void Reset()
        {
            Profile = null;
            convert = null;
            convert2 = null;

            ActiveRoot = new FsmBool { UseVariable = true };

            ActiveMode= new FsmBool { UseVariable = true };

            ActiveTemperature = new FsmBool { UseVariable = true };

            ActiveTint = new FsmBool { UseVariable = true };

            ActivePostExposureEV = new FsmBool { UseVariable = true };

            ActiveColorFilter = new FsmBool { UseVariable = true };

            ActiveHueShift = new FsmBool { UseVariable = true };

            ActiveSaturation = new FsmBool { UseVariable = true };

            ActiveContrast = new FsmBool { UseVariable = true };


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

                if (!ActiveRoot.IsNone)
                    colorGrading.active = ActiveRoot.Value;
                if (!ActiveMode.IsNone)
                    colorGrading.gradingMode.overrideState = ActiveMode.Value;
                if (!ActiveTemperature.IsNone)
                    colorGrading.temperature.overrideState = ActiveTemperature.Value;
                if (!ActiveTint.IsNone)
                    colorGrading.tint.overrideState = ActiveTint.Value;
                if (!ActivePostExposureEV.IsNone)
                    colorGrading.postExposure.overrideState = ActivePostExposureEV.Value;
                if (!ActiveColorFilter.IsNone)
                    colorGrading.colorFilter.overrideState = ActiveColorFilter.Value;
                if (!ActiveHueShift.IsNone)
                    colorGrading.hueShift.overrideState = ActiveHueShift.Value;
                if (!ActiveSaturation.IsNone)
                    colorGrading.saturation.overrideState = ActiveSaturation.Value;
                if (!ActiveContrast.IsNone)
                    colorGrading.contrast.overrideState = ActiveContrast.Value;

            }



        }
	}

}
