// Made by lovely Waveform
using UnityEngine.Rendering.PostProcessing;

namespace HutongGames.PlayMaker.Actions
{

	[ActionCategory("Post Processing V2")]
	public class GetActiveAmbientOcclusion : FsmStateAction
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

        //[ActionSection("Mode")]
        //public FsmBool GetMode;
        [UIHint(UIHint.Variable)]
        public FsmBool ModeValue;

        //[ActionSection("Intensity")]
        //public FsmBool GetIntensity;
        [UIHint(UIHint.Variable)]
        public FsmBool IntensityValue;

        //[ActionSection("Thickness Modifiler")]
        //public FsmBool GetThicknessModifier;
        [UIHint(UIHint.Variable)]
        public FsmBool ThicknessModifierValue;

        //[ActionSection("Color")]
        //public FsmBool GetColor;
        [UIHint(UIHint.Variable)]
        public FsmBool ColorValue;

        //[ActionSection("Ambient Only")]
        //public FsmBool GetAmbientOnly;
        [UIHint(UIHint.Variable)]
        public FsmBool AmbientOnlyValue;

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
            GetMode = false;
            GetIntensity = false;
            GetThicknessModifier = false;
            GetColor = false;
            GetAmbientOnly = false;
            */
            /*
            EnableValue = new FsmBool { UseVariable = true };
            ModeValue = new FsmBool { UseVariable = true };
            IntensityValue = new FsmBool { UseVariable = true };
            ThicknessModifierValue = new FsmBool { UseVariable = true };
            ColorValue = new FsmBool { UseVariable = true };
            AmbientOnlyValue = new FsmBool { UseVariable = true };
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
                convert.TryGetSettings(out AmbientOcclusion ambientOcclusion);


                if (!EnableValue.IsNone)
                    EnableValue.Value = ambientOcclusion.active;
                if (!ModeValue.IsNone)
                    ModeValue.Value = ambientOcclusion.mode.overrideState;
                if (!IntensityValue.IsNone)
                    IntensityValue.Value = ambientOcclusion.intensity.overrideState;
                if (!ThicknessModifierValue.IsNone)
                    ThicknessModifierValue.Value = ambientOcclusion.thicknessModifier.overrideState;
                if (!ColorValue.IsNone)
                    ColorValue.Value = ambientOcclusion.color.overrideState;
                if (!AmbientOnlyValue.IsNone)
                    AmbientOnlyValue.Value = ambientOcclusion.ambientOnly.value ;
            }



        }
	}

}
