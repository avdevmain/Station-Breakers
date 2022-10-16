// Made by lovely Waveform
using UnityEngine.Rendering.PostProcessing;

namespace HutongGames.PlayMaker.Actions
{

	[ActionCategory("Post Processing V2")]
	public class ActiveAmbientOcclusion : FsmStateAction
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

        //[ActionSection("Intensity")]
        public FsmBool ActiveIntensity;
        //public FsmBool IntensityValue;

        //[ActionSection("Thickness Modifiler")]
        public FsmBool ActiveThicknessModifier;
        //public FsmBool ThicknessModifierValue;

        //[ActionSection("Color")]
        public FsmBool ActiveColor;
        //public FsmBool ColorValue;

        //[ActionSection("Ambient Only")]
        public FsmBool ActiveAmbientOnly;
        //public FsmBool AmbientOnlyValue;

        [ActionSection(" ")]
        public bool everyFrame;


        private PostProcessProfile convert;
        private PostProcessVolume convert2;

        public override void Reset()
        {
            Profile = null;
            convert = null;
            convert2 = null;
            
            ActiveRoot = new FsmBool { UseVariable=true};
            ActiveMode = new FsmBool { UseVariable = true };
            ActiveIntensity = new FsmBool { UseVariable = true };
            ActiveThicknessModifier = new FsmBool { UseVariable = true };
            ActiveColor = new FsmBool { UseVariable = true };
            ActiveAmbientOnly = new FsmBool { UseVariable = true };
            

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
                convert.TryGetSettings(out AmbientOcclusion ambientOcclusion);

                if (!ActiveRoot.IsNone)
                    ambientOcclusion.active = ActiveRoot.Value;
                if (!ActiveMode.IsNone)
                    ambientOcclusion.mode.overrideState = ActiveMode.Value;
                if (!ActiveIntensity.IsNone)
                    ambientOcclusion.intensity.overrideState = ActiveIntensity.Value;
                if (!ActiveThicknessModifier.IsNone)
                    ambientOcclusion.thicknessModifier.overrideState = ActiveThicknessModifier.Value;
                if(!ActiveColor.IsNone)
                    ambientOcclusion.color.overrideState = ActiveColor.Value;
                if(!ActiveAmbientOnly.IsNone)
                    ambientOcclusion.ambientOnly.overrideState = ActiveAmbientOnly.Value;
            }



        }
	}

}
