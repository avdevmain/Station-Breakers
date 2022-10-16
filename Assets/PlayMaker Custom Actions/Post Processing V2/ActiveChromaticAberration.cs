// Made by lovely Waveform
using UnityEngine.Rendering.PostProcessing;

namespace HutongGames.PlayMaker.Actions
{

	[ActionCategory("Post Processing V2")]
	public class ActiveChromaticAberration : FsmStateAction
	{
        [ActionSection("Profile")]
        [ObjectType(typeof(PostProcessProfile))]
        public FsmObject Profile;
        [ObjectType(typeof(PostProcessVolume))]
        public FsmObject Volume;

        //[ActionSection("Root")]
        public FsmBool ActiveRoot;
        //public FsmBool RootValue;

        //[ActionSection("Spectral Lut")]
        public FsmBool ActiveSpectralLut;
        //public FsmBool SpectralLutValue;

        //[ActionSection("Intensity")]
        public FsmBool ActiveIntensity;
        //public FsmBool IntensityValue;

        //[ActionSection("FastMode")]
        public FsmBool ActiveFastMode;
        //public FsmBool FastModeValue;


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

            ActiveSpectralLut = new FsmBool { UseVariable = true };

            ActiveIntensity = new FsmBool { UseVariable = true };

            ActiveFastMode = new FsmBool { UseVariable = true };

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

                if (!ActiveRoot.IsNone)
                    chromaticAberration.active = ActiveRoot.Value;
                if (!ActiveSpectralLut.IsNone)
                    chromaticAberration.spectralLut.overrideState = ActiveSpectralLut.Value;
                if (!ActiveIntensity.IsNone)
                    chromaticAberration.intensity.overrideState = ActiveIntensity.Value;
                if (!ActiveFastMode.IsNone)
                    chromaticAberration.fastMode.overrideState = ActiveFastMode.Value;

            }



        }
	}

}
