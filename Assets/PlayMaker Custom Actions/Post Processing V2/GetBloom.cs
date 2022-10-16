// Made by lovely Waveform
using UnityEngine.Rendering.PostProcessing;

namespace HutongGames.PlayMaker.Actions
{

	[ActionCategory("Post Processing V2")]
	public class GetBloom : FsmStateAction
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

        //[ActionSection("Threshold")]
        //public FsmBool GetThreshold;
        [UIHint(UIHint.Variable)]
        public FsmFloat ThresholdValue;

        //[ActionSection("SoftKnee")]
        //public FsmBool GetSoftKnee;
        [UIHint(UIHint.Variable)]
        public FsmFloat SoftKneeValue;

        //[ActionSection("Clamp")]
        //public FsmBool GetClamp;
        [UIHint(UIHint.Variable)]
        public FsmFloat ClampValue;

        //[ActionSection("Diffusion")]
        //public FsmBool GetDiffusion;
        [UIHint(UIHint.Variable)]
        public FsmFloat DiffusionValue;

        //[ActionSection("Anamorphic Ratio")]
        //public FsmBool GetAnamorphicRatio;
        [UIHint(UIHint.Variable)]
        public FsmFloat AnamorphicRatioValue;

        //[ActionSection("Color")]
        //public FsmBool GetColor;
        [UIHint(UIHint.Variable)]
        public FsmColor ColorValue;

        //[ActionSection("Fast Mode")]
        //public FsmBool GetFastMode;
        [UIHint(UIHint.Variable)]
        public FsmBool FastModeValue;

        //[ActionSection("Dirt Texture")]
        //public FsmBool GetDirtTexture;
        [UIHint(UIHint.Variable)]
        public FsmTexture DirtTextureValue;

        //[ActionSection("Dirt Intensity")]
        //public FsmBool GetDirtIntensity;
        [UIHint(UIHint.Variable)]
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
            /*
            GetEnable = false;

            GetIntensity = false;

            GetThreshold = false;

            GetSoftKnee = false;

            GetClamp = false;

            GetDiffusion = false;

            GetAnamorphicRatio = false;

            GetColor = false;

            GetFastMode = false;

            GetDirtTexture = false;

            GetDirtIntensity = false;
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
                convert.TryGetSettings(out Bloom bloom);

                if (!EnableValue.IsNone)
                    EnableValue.Value = bloom.enabled.value;
                if (!IntensityValue.IsNone)
                    IntensityValue.Value = bloom.intensity.value;
                if (!ThresholdValue.IsNone)
                    ThresholdValue.Value = bloom.threshold.value;
                if (!SoftKneeValue.IsNone)
                    SoftKneeValue.Value = bloom.softKnee.value;
                if (!ClampValue.IsNone)
                    ClampValue.Value = bloom.clamp.value;
                if (!DiffusionValue.IsNone)
                    DiffusionValue.Value = bloom.diffusion.value;
                if (!AnamorphicRatioValue.IsNone)
                    AnamorphicRatioValue.Value = bloom.anamorphicRatio.value;
                if (!ColorValue.IsNone)
                    ColorValue.Value = bloom.color.value;
                if (!FastModeValue.IsNone)
                    FastModeValue.Value = bloom.fastMode.value;
                if (!DirtTextureValue.IsNone)
                    DirtTextureValue.Value = bloom.dirtTexture.value;
                if (!DirtIntensityValue.IsNone)
                    DirtIntensityValue.Value = bloom.dirtIntensity.value;

            }



        }
	}

}
