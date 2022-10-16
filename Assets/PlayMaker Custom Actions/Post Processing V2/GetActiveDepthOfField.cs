// Made by lovely Waveform
using UnityEngine.Rendering.PostProcessing;

namespace HutongGames.PlayMaker.Actions
{

	[ActionCategory("Post Processing V2")]
	public class GetActiveDepthOfField : FsmStateAction
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
        public FsmBool FocusDistanceValue;

        //[ActionSection("Aperture")]
        //public FsmBool GetAperture;
        [UIHint(UIHint.Variable)]
        public FsmBool ApertureValue;

        //[ActionSection("Focal Length")]
        //public FsmBool GetFocalLength;
        [UIHint(UIHint.Variable)]
        public FsmBool FocalLengthValue;

        //[ActionSection("Max Blur Size")]
        //public FsmBool GetMaxBlurSize;
        [UIHint(UIHint.Variable)]
        public FsmBool MaxBlurSizeValue;

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
                    EnableValue.Value=depthOfField.active;
                if (!FocusDistanceValue.IsNone)
                    FocusDistanceValue.Value=depthOfField.focusDistance.overrideState;
                if (!ApertureValue.IsNone)
                    ApertureValue.Value=depthOfField.aperture.overrideState;
                if (!FocalLengthValue.IsNone)
                    FocalLengthValue.Value=depthOfField.focalLength.overrideState;
                if (!MaxBlurSizeValue.IsNone)
                    MaxBlurSizeValue.Value=depthOfField.kernelSize.overrideState;



            }



        }
	}

}
