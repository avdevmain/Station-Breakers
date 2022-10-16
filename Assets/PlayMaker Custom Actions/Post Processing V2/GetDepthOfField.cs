// Made by lovely Waveform
using UnityEngine.Rendering.PostProcessing;

namespace HutongGames.PlayMaker.Actions
{

	[ActionCategory("Post Processing V2")]
	public class GetDepthOfField : FsmStateAction
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

        //[ActionSection("Focus Distance")]
        //public FsmBool GetFocusDistance;
        [UIHint(UIHint.Variable)]
        public FsmFloat FocusDistanceValue;

        //[ActionSection("Aperture")]
        //public FsmBool GetAperture;
        [UIHint(UIHint.Variable)]
        public FsmFloat ApertureValue;

        //[ActionSection("Focal Length")]
        //public FsmBool GetFocalLength;
        [UIHint(UIHint.Variable)]
        public FsmFloat FocalLengthValue;

        //[ActionSection("Max Blur Size")]
        //public FsmBool GetMaxBlurSize;
        [UIHint(UIHint.Variable)]
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
            /*
            GetEnable = false;
            EnableValue = false;

            GetFocusDistance = false;

            GetAperture = false;

            GetFocalLength = false;

            GetMaxBlurSize = false;
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
                convert.TryGetSettings(out DepthOfField depthOfField);

                if (!EnableValue.IsNone)
                    EnableValue.Value=depthOfField.enabled.value;
                if (!FocusDistanceValue.IsNone)
                    FocusDistanceValue.Value=depthOfField.focusDistance.value;
                if (!ApertureValue.IsNone)
                    ApertureValue.Value=depthOfField.aperture.value;
                if (!FocalLengthValue.IsNone)
                    FocalLengthValue.Value=depthOfField.focalLength.value;
                if (!MaxBlurSizeValue.IsNone)
                    MaxBlurSizeValue.Value=depthOfField.kernelSize.value;



            }



        }
	}

}
