// Made by lovely Waveform
using UnityEngine.Rendering.PostProcessing;

namespace HutongGames.PlayMaker.Actions
{

	[ActionCategory("Post Processing V2")]
	public class ActiveMotionBlur : FsmStateAction
	{
        [ActionSection("Profile")]
        [ObjectType(typeof(PostProcessProfile))]
        public FsmObject Profile;
        [ObjectType(typeof(PostProcessVolume))]
        public FsmObject Volume;

        //[ActionSection("Enable")]
        public FsmBool ActiveEnable;
        //public FsmBool EnableValue;

        //[ActionSection("Shutter Angle")]
        public FsmBool ActiveShutterAngle;
        //public FsmBool ShutterAngleValue;

        //[ActionSection("Sample Count")]
        public FsmBool ActiveSampleCount;
        //public FsmBool SampleCountValue;

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

            ActiveShutterAngle = new FsmBool { UseVariable = true };

            ActiveSampleCount = new FsmBool { UseVariable = true };

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
                convert.TryGetSettings(out MotionBlur motionBlur);

                if (!ActiveEnable.IsNone)
                    motionBlur.active = ActiveEnable.Value;
                if (!ActiveShutterAngle.IsNone)
                    motionBlur.shutterAngle.overrideState = ActiveShutterAngle.Value;
                if (!ActiveSampleCount.IsNone)
                    motionBlur.sampleCount.overrideState = ActiveSampleCount.Value;

            }



        }
	}

}
