// Made by lovely Waveform
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace HutongGames.PlayMaker.Actions
{

	[ActionCategory("Post Processing V2")]
	public class SetVignette : FsmStateAction
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
        [ObjectType(typeof(VignetteMode))]
        public FsmEnum ModeValue;

        [ActionSection("Color")]
        public FsmBool SetColor;
        public FsmColor ColorValue;

        [ActionSection("Center")]
        public FsmBool SetCenter;
        public FsmVector2 CenterValue;

        [ActionSection("Intensity")]
        public FsmBool SetIntensity;
        [HasFloatSlider(0,1)]
        public FsmFloat IntensityValue;

        [ActionSection("Smoothness")]
        public FsmBool SetSmoothness;
        [HasFloatSlider(0.01f,1)]
        public FsmFloat SmoothnessValue;

        [ActionSection("Roundness")]
        public FsmBool SetRoundness;
        [HasFloatSlider(0,1)]
        public FsmFloat RoundnessValue;

        [ActionSection("Rounded")]
        public FsmBool SetRounded;
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

            SetEnable = false;
            EnableValue = false;

            SetMode = false;
            ModeValue = VignetteMode.Classic;

            SetColor = false;
            ColorValue = default;

            SetCenter = false;
            CenterValue = new Vector2 (0.5f,0.5f);

            SetIntensity = false;
            IntensityValue = 0;

            SetSmoothness = false;
            SmoothnessValue = 0.2f;

            SetRoundness = false;
            RoundnessValue = 1;

            SetRounded = false;
            RoundedValue = false;

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

                if (SetEnable.Value)
                    vignette.enabled.value = EnableValue.Value;
                if (SetMode.Value)
                    vignette.mode.value = (VignetteMode)ModeValue.Value;
                if (SetColor.Value)
                    vignette.color.value = ColorValue.Value;
                if (SetCenter.Value)
                    vignette.center.value = CenterValue.Value;
                if (SetIntensity.Value)
                    vignette.intensity.value = IntensityValue.Value;
                if (SetSmoothness.Value)
                    vignette.smoothness.value = SmoothnessValue.Value;
                if (SetRoundness.Value)
                    vignette.roundness.value = RoundnessValue.Value;
                if (SetRounded.Value)
                    vignette.rounded.value = RoundedValue.Value;

            }



        }
	}

}
