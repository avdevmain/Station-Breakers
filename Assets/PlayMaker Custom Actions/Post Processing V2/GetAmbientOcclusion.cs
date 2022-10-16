// Made by lovely Waveform
using UnityEngine.Rendering.PostProcessing;

namespace HutongGames.PlayMaker.Actions
{

	[ActionCategory("Post Processing V2")]
	public class GetAmbientOcclusion : FsmStateAction
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
        [ObjectType(typeof(AmbientOcclusionMode))]
        public FsmEnum ModeValue;

        //[ActionSection("Intensity")]
        //public FsmBool GetIntensity;
        [UIHint(UIHint.Variable)]
        public FsmFloat IntensityValue;

        //[ActionSection("Thickness Modifiler")]
        //public FsmBool GetThicknessModifier;
        [UIHint(UIHint.Variable)]
        public FsmFloat ThicknessModifierValue;

        //[ActionSection("Color")]
        //public FsmBool GetColor;
        [UIHint(UIHint.Variable)]
        public FsmColor ColorValue;

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
            EnableValue = false;
            GetMode = false;
            ModeValue = default;
            GetIntensity = false;
            IntensityValue = 0;
            GetThicknessModifier = false;
            ThicknessModifierValue = 1;
            GetColor = false;
            ColorValue = default;
            GetAmbientOnly = false;
            AmbientOnlyValue = false;
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
                    EnableValue.Value = ambientOcclusion.enabled.value;
                if (!ModeValue.IsNone)
                    ModeValue.Value = ambientOcclusion.mode.value;
                if (!IntensityValue.IsNone)
                    IntensityValue.Value = ambientOcclusion.intensity.value;
                if (!ThicknessModifierValue.IsNone)
                    ThicknessModifierValue.Value = ambientOcclusion.thicknessModifier.value;
                if (!ColorValue.IsNone)
                    ColorValue.Value = ambientOcclusion.color.value;
                if (!AmbientOnlyValue.IsNone)
                    AmbientOnlyValue.Value = ambientOcclusion.ambientOnly.value ;
            }



        }
	}

}
