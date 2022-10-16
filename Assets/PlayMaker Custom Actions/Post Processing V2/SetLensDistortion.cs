// Made by lovely Waveform
using UnityEngine.Rendering.PostProcessing;

namespace HutongGames.PlayMaker.Actions
{

	[ActionCategory("Post Processing V2")]
	public class SetLensDistortion : FsmStateAction
	{
        [ActionSection("Profile")]
        [ObjectType(typeof(PostProcessProfile))]
        public FsmObject Profile;
        [ObjectType(typeof(PostProcessVolume))]
        public FsmObject Volume;

        [ActionSection("Enable")]
        public FsmBool SetEnable;
        public FsmBool EnableValue;

        [ActionSection("Intensity")]
        public FsmBool SetIntensity;
        [HasFloatSlider(-100,100)]
        public FsmFloat IntensityValue;

        [ActionSection("X Multiplier")]
        public FsmBool SetXMultiplier;
        [HasFloatSlider(0,1)]
        public FsmFloat XMultiplierValue;

        [ActionSection("Y Multiplier")]
        public FsmBool SetYMultiplier;
        [HasFloatSlider(0, 1)]
        public FsmFloat YMultiplierValue;

        [ActionSection("CenterX")]
        public FsmBool SetCenterX;
        [HasFloatSlider(-1, 1)]
        public FsmFloat CenterXValue;

        [ActionSection("CenterY")]
        public FsmBool SetCenterY;
        [HasFloatSlider(-1, 1)]
        public FsmFloat CenterYValue;

        [ActionSection("Scale")]
        public FsmBool SetScale;
        [HasFloatSlider(0.01f, 5)]
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

            SetEnable = false;
            EnableValue = false;

            SetIntensity = false;
            IntensityValue = 0;

            SetXMultiplier = false;
            XMultiplierValue = 1;

            SetYMultiplier = false;
            YMultiplierValue = 1;

            SetCenterX = false;
            CenterXValue = 0;

            SetCenterY = false;
            CenterYValue = 0;

            SetScale = false;
            ScaleValue = 1;

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
                convert.TryGetSettings(out LensDistortion lensDistortion);

                if (SetEnable.Value)
                    lensDistortion.enabled.value = EnableValue.Value;
                if (SetIntensity.Value)
                    lensDistortion.intensity.value = IntensityValue.Value;
                if (SetXMultiplier.Value)
                    lensDistortion.intensityX.value = XMultiplierValue.Value;
                if (SetYMultiplier.Value)
                    lensDistortion.intensityY.value = YMultiplierValue.Value;
                if (SetCenterX.Value)
                    lensDistortion.centerX.value = CenterXValue.Value;
                if (SetCenterY.Value)
                    lensDistortion.centerY.value = CenterYValue.Value;
                if (SetScale.Value)
                    lensDistortion.scale.value = ScaleValue.Value;


            }



        }
	}

}
