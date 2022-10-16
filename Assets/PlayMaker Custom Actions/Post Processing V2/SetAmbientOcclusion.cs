// Made by lovely Waveform
using UnityEngine.Rendering.PostProcessing;

namespace HutongGames.PlayMaker.Actions
{

	[ActionCategory("Post Processing V2")]
	public class SetAmbientOcclusion : FsmStateAction
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
        [ObjectType(typeof(AmbientOcclusionMode))]
        public FsmEnum ModeValue;

        [ActionSection("Intensity")]
        public FsmBool SetIntensity;
        [HasFloatSlider(0,4)]
        public FsmFloat IntensityValue;

        [ActionSection("Thickness Modifiler")]
        public FsmBool SetThicknessModifier;
        [HasFloatSlider(1, 10)]
        public FsmFloat ThicknessModifierValue;

        [ActionSection("Color")]
        public FsmBool SetColor;
        public FsmColor ColorValue;

        [ActionSection("Ambient Only")]
        public FsmBool SetAmbientOnly;
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

            SetEnable = false;
            EnableValue = false;

            SetMode = false;
            ModeValue = AmbientOcclusionMode.MultiScaleVolumetricObscurance;

            SetIntensity = false;
            IntensityValue = 0;

            SetThicknessModifier = false;
            ThicknessModifierValue = 1;

            SetColor = false;
            ColorValue = default;

            SetAmbientOnly = false;
            AmbientOnlyValue = false;

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

                if (SetEnable.Value)
                    ambientOcclusion.enabled.value = EnableValue.Value;
                if (SetMode.Value)
                    ambientOcclusion.mode.value = (AmbientOcclusionMode)ModeValue.Value;
                if (SetIntensity.Value)
                    ambientOcclusion.intensity.value = IntensityValue.Value;
                if (SetThicknessModifier.Value)
                    ambientOcclusion.thicknessModifier.value = ThicknessModifierValue.Value;
                if(SetColor.Value)
                    ambientOcclusion.color.value = ColorValue.Value;
                if(SetAmbientOnly.Value)
                    ambientOcclusion.ambientOnly.value = AmbientOnlyValue.Value;
            }



        }
	}

}
