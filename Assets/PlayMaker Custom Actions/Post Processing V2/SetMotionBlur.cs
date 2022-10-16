// Made by lovely Waveform
using UnityEngine.Rendering.PostProcessing;

namespace HutongGames.PlayMaker.Actions
{

	[ActionCategory("Post Processing V2")]
	public class SetMotionBlur : FsmStateAction
	{
        [ActionSection("Profile")]
        [ObjectType(typeof(PostProcessProfile))]
        public FsmObject Profile;
        [ObjectType(typeof(PostProcessVolume))]
        public FsmObject Volume;

        [ActionSection("Enable")]
        public FsmBool SetEnable;
        public FsmBool EnableValue;

        [ActionSection("Shutter Angle")]
        public FsmBool SetShutterAngle;
        [HasFloatSlider(0,360)]
        public FsmFloat ShutterAngleValue;

        [ActionSection("Sample Count")]
        public FsmBool SetSampleCount;
        public FsmInt SampleCountValue;

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

            SetShutterAngle = false;
            ShutterAngleValue = 270;

            SetSampleCount = false;
            SampleCountValue = 10;

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

                if (SetEnable.Value)
                    motionBlur.enabled.value = EnableValue.Value;
                if (SetShutterAngle.Value)
                    motionBlur.shutterAngle.value = ShutterAngleValue.Value;
                if (SetSampleCount.Value)
                    motionBlur.sampleCount.value = SampleCountValue.Value;

            }



        }
	}

}
