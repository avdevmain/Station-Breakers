// Made by lovely Waveform
using UnityEngine.Rendering.PostProcessing;

namespace HutongGames.PlayMaker.Actions
{

	[ActionCategory("Post Processing V2")]
	public class SetDepthOfField : FsmStateAction
	{
        [ActionSection("Profile")]
        [ObjectType(typeof(PostProcessProfile))]
        public FsmObject Profile;
        [ObjectType(typeof(PostProcessVolume))]
        public FsmObject Volume;

        [ActionSection("Enable")]
        public FsmBool SetEnable;
        public FsmBool EnableValue;

        [ActionSection("Focus Distance")]
        public FsmBool SetFocusDistance;
        public FsmFloat FocusDistanceValue;

        [ActionSection("Aperture")]
        public FsmBool SetAperture;
        [HasFloatSlider(0.1f,32)]
        public FsmFloat ApertureValue;

        [ActionSection("Focal Length")]
        public FsmBool SetFocalLength;
        [HasFloatSlider(1, 300)]
        public FsmFloat FocalLengthValue;

        [ActionSection("Max Blur Size")]
        public FsmBool SetMaxBlurSize;
        [ObjectType(typeof(KernelSize))]
        public FsmEnum MaxBlurSizeValue;

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

            SetFocusDistance = false;
            FocusDistanceValue = 10;

            SetAperture = false;
            ApertureValue = 5.6f;

            SetFocalLength = false;
            FocalLengthValue = 50;

            SetMaxBlurSize = false;
            MaxBlurSizeValue = KernelSize.Medium;

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
                convert.TryGetSettings(out DepthOfField depthOfField);

                if (SetEnable.Value)
                    depthOfField.enabled.value = EnableValue.Value;
                if (SetFocusDistance.Value)
                    depthOfField.focusDistance.value = FocusDistanceValue.Value;
                if (SetAperture.Value)
                    depthOfField.aperture.value = ApertureValue.Value;
                if (SetFocalLength.Value)
                    depthOfField.focalLength.value = FocalLengthValue.Value;
                if (SetMaxBlurSize.Value)
                    depthOfField.kernelSize.value = (KernelSize)MaxBlurSizeValue.Value;



            }



        }
	}

}
