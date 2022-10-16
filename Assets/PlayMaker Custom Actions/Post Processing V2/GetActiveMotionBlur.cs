// Made by lovely Waveform
using UnityEngine.Rendering.PostProcessing;

namespace HutongGames.PlayMaker.Actions
{

	[ActionCategory("Post Processing V2")]
	public class GetActiveMotionBlur : FsmStateAction
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

        //[ActionSection("Shutter Angle")]
        //public FsmBool GetShutterAngle;
        [UIHint(UIHint.Variable)]
        public FsmBool ShutterAngleValue;

        //[ActionSection("Sample Count")]
        //public FsmBool GetSampleCount;
        [UIHint(UIHint.Variable)]
        public FsmBool SampleCountValue;

        [ActionSection(" ")]
        public bool everyFrame;


        private PostProcessProfile convert;
        private PostProcessVolume convert2;

        public override void Reset()
        {
            Profile = null;
            convert = null;
            convert2 = null;

            //GetEnable = false;
            //GetShutterAngle = false;
            //GetSampleCount = false;

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
                convert.TryGetSettings(out MotionBlur motionBlur);

                if (!EnableValue.IsNone)
                    EnableValue.Value=motionBlur.active;
                if (!ShutterAngleValue.IsNone)
                    ShutterAngleValue.Value=motionBlur.shutterAngle.overrideState;
                if (!SampleCountValue.IsNone)
                    SampleCountValue.Value=motionBlur.sampleCount.overrideState;

            }



        }
	}

}
