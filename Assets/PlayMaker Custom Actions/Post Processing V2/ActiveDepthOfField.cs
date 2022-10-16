// Made by lovely Waveform
using UnityEngine.Rendering.PostProcessing;

namespace HutongGames.PlayMaker.Actions
{

	[ActionCategory("Post Processing V2")]
	public class ActiveDepthOfField : FsmStateAction
	{
        [ActionSection("Profile")]
        [ObjectType(typeof(PostProcessProfile))]
        public FsmObject Profile;
        [ObjectType(typeof(PostProcessVolume))]
        public FsmObject Volume;

        //[ActionSection("Enable")]
        public FsmBool ActiveEnable;
        //public FsmBool EnableValue;

        //[ActionSection("Focus Distance")]
        public FsmBool ActiveFocusDistance;
        //public FsmBool FocusDistanceValue;

        //[ActionSection("Aperture")]
        public FsmBool ActiveAperture;
        //public FsmBool ApertureValue;

        //[ActionSection("Focal Length")]
        public FsmBool ActiveFocalLength;
        //public FsmBool FocalLengthValue;

        //[ActionSection("Max Blur Size")]
        public FsmBool ActiveMaxBlurSize;
        //public FsmBool MaxBlurSizeValue;

        [ActionSection(" ")]
        public bool everyFrame;


        private PostProcessProfile convert;
        private PostProcessVolume convert2;

        public override void Reset()
        {
            Profile = null;
            convert = null;
            convert2 = null;

            ActiveEnable = new FsmBool { UseVariable = true };

            ActiveFocusDistance = new FsmBool { UseVariable = true };

            ActiveAperture = new FsmBool { UseVariable = true };

            ActiveFocalLength = new FsmBool { UseVariable = true };

            ActiveMaxBlurSize = new FsmBool { UseVariable = true };

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

                if (!ActiveEnable.IsNone)
                    depthOfField.active = ActiveEnable.Value;
                if (!ActiveFocusDistance.IsNone)
                    depthOfField.focusDistance.overrideState = ActiveFocusDistance.Value;
                if (!ActiveAperture.IsNone)
                    depthOfField.aperture.overrideState = ActiveAperture.Value;
                if (!ActiveFocalLength.IsNone)
                    depthOfField.focalLength.overrideState = ActiveFocalLength.Value;
                if (!ActiveMaxBlurSize.IsNone)
                    depthOfField.kernelSize.overrideState = ActiveMaxBlurSize.Value;



            }



        }
	}

}
