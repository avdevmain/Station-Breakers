// Made by lovely Waveform
using UnityEngine.Rendering.PostProcessing;

namespace HutongGames.PlayMaker.Actions
{

	[ActionCategory("Post Processing V2")]
	public class ActiveBloom : FsmStateAction
	{
        [ActionSection("Profile")]
        [ObjectType(typeof(PostProcessProfile))]
        public FsmObject Profile;
        [ObjectType(typeof(PostProcessVolume))]
        public FsmObject Volume;

        //[ActionSection("Root")]
        public FsmBool ActiveRoot;
        //public FsmBool RootValue;

        //[ActionSection("Intensity")]
        public FsmBool ActiveIntensity;
        //public FsmBool IntensityValue;

        //[ActionSection("Threshold")]
        public FsmBool ActiveThreshold;
        //public FsmBool ThresholdValue;

        //[ActionSection("SoftKnee")]
        public FsmBool ActiveSoftKnee;
        //public FsmBool SoftKneeValue;

        //[ActionSection("Clamp")]
        public FsmBool ActiveClamp;
        //public FsmBool ClampValue;

        //[ActionSection("Diffusion")]
        public FsmBool ActiveDiffusion;
        //public FsmBool DiffusionValue;

        //[ActionSection("Anamorphic Ratio")]
        public FsmBool ActiveAnamorphicRatio;
        //public FsmBool AnamorphicRatioValue;

        //[ActionSection("Color")]
        public FsmBool ActiveColor;
        //public FsmBool ColorValue;

        //[ActionSection("Fast Mode")]
        public FsmBool ActiveFastMode;
        //public FsmBool FastModeValue;

        //[ActionSection("Dirt Texture")]
        public FsmBool ActiveDirtTexture;
        //public FsmBool DirtTextureValue;

        //[ActionSection("Dirt Intensity")]
        public FsmBool ActiveDirtIntensity;
        //public FsmBool DirtIntensityValue;


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

            ActiveIntensity = new FsmBool { UseVariable = true };

            ActiveThreshold = new FsmBool { UseVariable = true };

            ActiveSoftKnee = new FsmBool { UseVariable = true };

            ActiveClamp = new FsmBool { UseVariable = true };

            ActiveDiffusion = new FsmBool { UseVariable = true };

            ActiveAnamorphicRatio = new FsmBool { UseVariable = true };

            ActiveColor = new FsmBool { UseVariable = true };

            ActiveFastMode = new FsmBool { UseVariable = true };

            ActiveDirtTexture = new FsmBool { UseVariable = true };

            ActiveDirtIntensity = new FsmBool { UseVariable = true };

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

                if (!ActiveRoot.IsNone)
                    bloom.active = ActiveRoot.Value;
                if (!ActiveIntensity.IsNone)
                    bloom.intensity.overrideState = ActiveIntensity.Value;
                if (!ActiveThreshold.IsNone)
                    bloom.threshold.overrideState = ActiveThreshold.Value;
                if (!ActiveSoftKnee.IsNone)
                    bloom.softKnee.overrideState = ActiveSoftKnee.Value;
                if (!ActiveClamp.IsNone)
                    bloom.clamp.overrideState = ActiveClamp.Value;
                if (!ActiveDiffusion.IsNone)
                    bloom.diffusion.overrideState = ActiveDiffusion.Value;
                if (!ActiveAnamorphicRatio.IsNone)
                    bloom.anamorphicRatio.overrideState = ActiveAnamorphicRatio.Value;
                if (!ActiveColor.IsNone)
                    bloom.color.overrideState = ActiveColor.Value;
                if (!ActiveFastMode.IsNone)
                    bloom.fastMode.overrideState = ActiveFastMode.Value;
                if (!ActiveDirtTexture.IsNone)
                    bloom.dirtTexture.overrideState = ActiveDirtTexture.Value;
                if (!ActiveDirtIntensity.IsNone)
                    bloom.dirtIntensity.overrideState = ActiveDirtIntensity.Value;

            }



        }
	}

}
