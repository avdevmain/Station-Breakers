// Made by lovely Waveform
using UnityEngine.Rendering.PostProcessing;

namespace HutongGames.PlayMaker.Actions
{

	[ActionCategory("Post Processing V2")]
	public class GetActiveBloom : FsmStateAction
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
        public FsmBool IntensityValue;

        //[ActionSection("Threshold")]
        //public FsmBool GetThreshold;
        [UIHint(UIHint.Variable)]
        public FsmBool ThresholdValue;

        //[ActionSection("SoftKnee")]
        //public FsmBool GetSoftKnee;
        [UIHint(UIHint.Variable)]
        public FsmBool SoftKneeValue;

        //[ActionSection("Clamp")]
        //public FsmBool GetClamp;
        [UIHint(UIHint.Variable)]
        public FsmBool ClampValue;

        //[ActionSection("Diffusion")]
        //public FsmBool GetDiffusion;
        [UIHint(UIHint.Variable)]
        public FsmBool DiffusionValue;

        //[ActionSection("Anamorphic Ratio")]
        //public FsmBool GetAnamorphicRatio;
        [UIHint(UIHint.Variable)]
        public FsmBool AnamorphicRatioValue;

        //[ActionSection("Color")]
        //public FsmBool GetColor;
        [UIHint(UIHint.Variable)]
        public FsmBool ColorValue;

        //[ActionSection("Fast Mode")]
        //public FsmBool GetFastMode;
        [UIHint(UIHint.Variable)]
        public FsmBool FastModeValue;

        //[ActionSection("Dirt Texture")]
        //public FsmBool GetDirtTexture;
        [UIHint(UIHint.Variable)]
        public FsmBool DirtTextureValue;

        //[ActionSection("Dirt Intensity")]
        //public FsmBool GetDirtIntensity;
        [UIHint(UIHint.Variable)]
        public FsmBool DirtIntensityValue;


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
            /*
            EnableValue = new FsmBool { UseVariable = true };
            IntensityValue = new FsmBool { UseVariable = true };
            ThresholdValue = new FsmBool { UseVariable = true };
            SoftKneeValue = new FsmBool { UseVariable = true };
            ClampValue = new FsmBool { UseVariable = true };
            DiffusionValue = new FsmBool { UseVariable = true };
            AnamorphicRatioValue = new FsmBool { UseVariable = true };
            ColorValue = new FsmBool { UseVariable = true };
            FastModeValue = new FsmBool { UseVariable = true };
            DirtTextureValue = new FsmBool { UseVariable = true };
            DirtIntensityValue = new FsmBool { UseVariable = true };
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
                    EnableValue.Value = bloom.active;
                if (!IntensityValue.IsNone)
                    IntensityValue.Value = bloom.intensity.overrideState;
                if (!ThresholdValue.IsNone)
                    ThresholdValue.Value = bloom.threshold.overrideState;
                if (!SoftKneeValue.IsNone)
                    SoftKneeValue.Value = bloom.softKnee.overrideState;
                if (!ClampValue.IsNone)
                    ClampValue.Value = bloom.clamp.overrideState;
                if (!DiffusionValue.IsNone)
                    DiffusionValue.Value = bloom.diffusion.overrideState;
                if (!AnamorphicRatioValue.IsNone)
                    AnamorphicRatioValue.Value = bloom.anamorphicRatio.overrideState;
                if (!ColorValue.IsNone)
                    ColorValue.Value = bloom.color.overrideState;
                if (!FastModeValue.IsNone)
                    FastModeValue.Value = bloom.fastMode.overrideState;
                if (!DirtTextureValue.IsNone)
                    DirtTextureValue.Value = bloom.dirtTexture.overrideState;
                if (!DirtIntensityValue.IsNone)
                    DirtIntensityValue.Value = bloom.dirtIntensity.overrideState;

            }



        }
	}

}
