// Made by lovely Waveform
using UnityEngine.Rendering.PostProcessing;

namespace HutongGames.PlayMaker.Actions
{

	[ActionCategory("Post Processing V2")]
	public class ActiveColorGradingMixer : FsmStateAction
	{
        [ActionSection("Profile")]
        [ObjectType(typeof(PostProcessProfile))]
        public FsmObject Profile;
        [ObjectType(typeof(PostProcessVolume))]
        public FsmObject Volume;

        [ActionSection("Enable")]
        public FsmBool ActiveEnable;
        public FsmBool EnableValue;

        [ActionSection("Channel Mixer Red")]
        public FsmBool ActiveRed;
        public FsmBool RedValue;
        public FsmBool GreenValue;
        public FsmBool BlueValue;

        [ActionSection("Channel Mixer Green")]
        public FsmBool ActiveGreen;
        public FsmBool RedValue_;
        public FsmBool GreenValue_;
        public FsmBool BlueValue_;

        [ActionSection("Channel Mixer Blue")]
        public FsmBool ActiveBlue;
        public FsmBool Red_Value;
        public FsmBool Green_Value;
        public FsmBool Blue_Value;


        [ActionSection(" ")]
        public bool everyFrame;


        private PostProcessProfile convert;
        private PostProcessVolume convert2;

        public override void Reset()
        {
            Profile = null;
            convert = null;
            convert2 = null;

            ActiveEnable = false;
            EnableValue = false;

            ActiveRed = false;
            ActiveBlue = false;
            ActiveGreen = false;

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
                convert.TryGetSettings(out ColorGrading colorGrading);

                if (ActiveEnable.Value)
                    colorGrading.active = EnableValue.Value;
                if (ActiveRed.Value)
                {
                    colorGrading.mixerRedOutRedIn.overrideState = RedValue.Value;
                    colorGrading.mixerRedOutGreenIn.overrideState = GreenValue.Value;
                    colorGrading.mixerRedOutBlueIn.overrideState = BlueValue.Value;
                }
                if (ActiveGreen.Value)
                {
                    colorGrading.mixerGreenOutRedIn.overrideState = RedValue_.Value;
                    colorGrading.mixerGreenOutGreenIn.overrideState = GreenValue_.Value;
                    colorGrading.mixerGreenOutBlueIn.overrideState = BlueValue_.Value;
                }
                if (ActiveBlue.Value)
                {
                    colorGrading.mixerBlueOutRedIn.overrideState = Red_Value.Value;
                    colorGrading.mixerBlueOutGreenIn.overrideState = Green_Value.Value;
                    colorGrading.mixerBlueOutBlueIn.overrideState = Blue_Value.Value;
                }



            }



        }
	}

}
