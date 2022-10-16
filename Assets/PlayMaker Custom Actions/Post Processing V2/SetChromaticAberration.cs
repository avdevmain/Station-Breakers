// Made by lovely Waveform
using UnityEngine.Rendering.PostProcessing;

namespace HutongGames.PlayMaker.Actions
{

	[ActionCategory("Post Processing V2")]
	public class SetChromaticAberration : FsmStateAction
	{
        [ActionSection("Profile")]
        [ObjectType(typeof(PostProcessProfile))]
        public FsmObject Profile;
        [ObjectType(typeof(PostProcessVolume))]
        public FsmObject Volume;

        [ActionSection("Enable")]
        public FsmBool SetEnable;
        public FsmBool EnableValue;

        [ActionSection("Spectral Lut")]
        public FsmBool SetSpectralLut;
        public FsmTexture SpectralLutValue;

        [ActionSection("Intensity")]
        public FsmBool SetIntensity;
        [HasFloatSlider(0,1)]
        public FsmFloat IntensityValue;

        [ActionSection("FastMode")]
        public FsmBool SetFastMode;
        public FsmBool FastModeValue;


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

            SetSpectralLut = false;
            SpectralLutValue = null;

            SetIntensity = false;
            IntensityValue = 0;

            SetFastMode = false;
            FastModeValue = false;

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
                convert.TryGetSettings(out ChromaticAberration chromaticAberration);

                if (SetEnable.Value)
                    chromaticAberration.enabled.value = EnableValue.Value;
                if (SetSpectralLut.Value)
                    chromaticAberration.spectralLut.value = SpectralLutValue.Value;
                if (SetIntensity.Value)
                    chromaticAberration.intensity.value = IntensityValue.Value;
                if (SetFastMode.Value)
                    chromaticAberration.fastMode.value = FastModeValue.Value;

            }



        }
	}

}
