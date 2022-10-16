// Made by lovely Waveform
using UnityEngine.Rendering.PostProcessing;

namespace HutongGames.PlayMaker.Actions
{

	[ActionCategory("Post Processing V2")]
	public class SetBloom : FsmStateAction
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
        public FsmFloat IntensityValue;

        [ActionSection("Threshold")]
        public FsmBool SetThreshold;
        public FsmFloat ThresholdValue;

        [ActionSection("SoftKnee")]
        public FsmBool SetSoftKnee;
        [HasFloatSlider(0, 1)]
        public FsmFloat SoftKneeValue;

        [ActionSection("Clamp")]
        public FsmBool SetClamp;
        public FsmFloat ClampValue;

        [ActionSection("Diffusion")]
        public FsmBool SetDiffusion;
        [HasFloatSlider(1, 10)]
        public FsmFloat DiffusionValue;

        [ActionSection("Anamorphic Ratio")]
        public FsmBool SetAnamorphicRatio;
        [HasFloatSlider(-1, 1)]
        public FsmFloat AnamorphicRatioValue;

        [ActionSection("Color")]
        public FsmBool SetColor;
        public FsmColor ColorValue;

        [ActionSection("Fast Mode")]
        public FsmBool SetFastMode;
        public FsmBool FastModeValue;

        [ActionSection("Dirt Texture")]
        public FsmBool SetDirtTexture;
        public FsmTexture DirtTextureValue;

        [ActionSection("Dirt Intensity")]
        public FsmBool SetDirtIntensity;
        public FsmFloat DirtIntensityValue;


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

            SetThreshold = false;
            ThresholdValue = 1;

            SetSoftKnee = false;
            SoftKneeValue = 0.5f;

            SetClamp = false;
            ClampValue = 65472;

            SetDiffusion = false;
            DiffusionValue = 7;

            SetAnamorphicRatio = false;
            AnamorphicRatioValue = 0;

            SetColor = false;
            ColorValue = default;

            SetFastMode = false;
            FastModeValue = false;

            SetDirtTexture = false;
            SetDirtTexture = null;

            SetDirtIntensity = false;
            DirtIntensityValue = 0;

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
                convert.TryGetSettings(out Bloom bloom);

                if (SetEnable.Value)
                    bloom.enabled.value = EnableValue.Value;
                if (SetIntensity.Value)
                    bloom.intensity.value = IntensityValue.Value;
                if (SetThreshold.Value)
                    bloom.threshold.value = ThresholdValue.Value;
                if (SetSoftKnee.Value)
                    bloom.softKnee.value = SoftKneeValue.Value;
                if (SetClamp.Value)
                    bloom.clamp.value = ClampValue.Value;
                if (SetDiffusion.Value)
                    bloom.diffusion.value = DiffusionValue.Value;
                if (SetAnamorphicRatio.Value)
                    bloom.anamorphicRatio.value = AnamorphicRatioValue.Value;
                if (SetColor.Value)
                    bloom.color.value = ColorValue.Value;
                if (SetFastMode.Value)
                    bloom.fastMode.value = FastModeValue.Value;
                if (SetDirtTexture.Value)
                    bloom.dirtTexture.value = DirtTextureValue.Value;
                if (SetDirtIntensity.Value)
                    bloom.dirtIntensity.value = DirtIntensityValue.Value;

            }



        }
	}

}
