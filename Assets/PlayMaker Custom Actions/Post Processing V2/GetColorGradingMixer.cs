// Made by lovely Waveform
using UnityEngine.Rendering.PostProcessing;

namespace HutongGames.PlayMaker.Actions
{

	[ActionCategory("Post Processing V2")]
	public class GetColorGradingMixer : FsmStateAction
	{
        [ActionSection("Profile")]
        [ObjectType(typeof(PostProcessProfile))]
        public FsmObject Profile;
        [ObjectType(typeof(PostProcessVolume))]
        public FsmObject Volume;
        [UIHint(UIHint.Variable)]
        [ObjectType(typeof(PostProcessVolume))]
        public FsmObject VolumeProfile;

        [ActionSection("Enable")]
        public FsmBool GetEnable;
        [UIHint(UIHint.Variable)]
        public FsmBool EnableValue;

        [ActionSection("Channel Mixer Red")]
        public FsmBool GetRed;
        [UIHint(UIHint.Variable)]
        public FsmFloat RedValue;
        [UIHint(UIHint.Variable)]
        public FsmFloat GreenValue;
        [UIHint(UIHint.Variable)]
        public FsmFloat BlueValue;

        [ActionSection("Channel Mixer Green")]
        public FsmBool GetGreen;
        [UIHint(UIHint.Variable)]
        public FsmFloat RedValue_;
        [UIHint(UIHint.Variable)]
        public FsmFloat GreenValue_;
        [UIHint(UIHint.Variable)]
        public FsmFloat BlueValue_;

        [ActionSection("Channel Mixer Blue")]
        public FsmBool GetBlue;
        [UIHint(UIHint.Variable)]
        public FsmFloat Red_Value;
        [UIHint(UIHint.Variable)]
        public FsmFloat Green_Value;
        [UIHint(UIHint.Variable)]
        public FsmFloat Blue_Value;


        [ActionSection(" ")]
        public bool everyFrame;


        private PostProcessProfile convert;
        private PostProcessVolume convert2;

        public override void Reset()
        {
            Profile = null;
            convert = null;
            convert2 = null;

            GetEnable = false;
            EnableValue = false;

            GetRed = false;
            GetBlue = false;
            GetGreen = false;

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
                convert.TryGetSettings(out ColorGrading colorGrading);

                if (GetEnable.Value)
                    EnableValue.Value=colorGrading.enabled.value;
                if (GetRed.Value)
                {
                    RedValue.Value=colorGrading.mixerRedOutRedIn.value;
                    GreenValue.Value=colorGrading.mixerRedOutGreenIn.value;
                    BlueValue.Value=colorGrading.mixerRedOutBlueIn.value;
                }
                if (GetGreen.Value)
                {
                    RedValue_.Value=colorGrading.mixerGreenOutRedIn.value;
                    GreenValue_.Value=colorGrading.mixerGreenOutGreenIn.value;
                    BlueValue_.Value=colorGrading.mixerGreenOutBlueIn.value;
                }
                if (GetBlue.Value)
                {
                    Red_Value.Value=colorGrading.mixerBlueOutRedIn.value;
                    Green_Value.Value=colorGrading.mixerBlueOutGreenIn.value;
                    Blue_Value.Value=colorGrading.mixerBlueOutBlueIn.value;
                }



            }



        }
	}

}
