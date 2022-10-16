// Made by lovely Waveform
using UnityEngine.Rendering.PostProcessing;

namespace HutongGames.PlayMaker.Actions
{

	[ActionCategory("Post Processing V2")]
	public class SetColorGradingMixer : FsmStateAction
	{
        [ActionSection("Profile")]
        [ObjectType(typeof(PostProcessProfile))]
        public FsmObject Profile;
        [ObjectType(typeof(PostProcessVolume))]
        public FsmObject Volume;

        [ActionSection("Enable")]
        public FsmBool SetEnable;
        public FsmBool EnableValue;

        [ActionSection("Channel Mixer Red")]
        public FsmBool SetRed;
        [HasFloatSlider(-200, 200)]
        public FsmFloat RedValue;
        [HasFloatSlider(-200, 200)]
        public FsmFloat GreenValue;
        [HasFloatSlider(-200, 200)]
        public FsmFloat BlueValue;

        [ActionSection("Channel Mixer Green")]
        public FsmBool SetGreen;
        [HasFloatSlider(-200, 200)]
        public FsmFloat RedValue_;
        [HasFloatSlider(-200, 200)]
        public FsmFloat GreenValue_;
        [HasFloatSlider(-200, 200)]
        public FsmFloat BlueValue_;

        [ActionSection("Channel Mixer Blue")]
        public FsmBool SetBlue;
        [HasFloatSlider(-200, 200)]
        public FsmFloat Red_Value;
        [HasFloatSlider(-200, 200)]
        public FsmFloat Green_Value;
        [HasFloatSlider(-200, 200)]
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

            SetEnable = false;
            EnableValue = false;

            SetRed = false;
            SetBlue = false;
            SetGreen = false;

            RedValue = 100;
            RedValue_ = 0;
            Red_Value = 0;

            GreenValue = 0;
            GreenValue_ = 100;
            Green_Value = 0;

            BlueValue = 0;
            BlueValue_ = 0;
            Blue_Value = 100;

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

                if (SetEnable.Value)
                    colorGrading.enabled.value = EnableValue.Value;
                if (SetRed.Value)
                {
                    colorGrading.mixerRedOutRedIn.value = RedValue.Value;
                    colorGrading.mixerRedOutGreenIn.value = GreenValue.Value;
                    colorGrading.mixerRedOutBlueIn.value = BlueValue.Value;
                }
                if (SetGreen.Value)
                {
                    colorGrading.mixerGreenOutRedIn.value = RedValue_.Value;
                    colorGrading.mixerGreenOutGreenIn.value = GreenValue_.Value;
                    colorGrading.mixerGreenOutBlueIn.value = BlueValue_.Value;
                }
                if (SetBlue.Value)
                {
                    colorGrading.mixerBlueOutRedIn.value = Red_Value.Value;
                    colorGrading.mixerBlueOutGreenIn.value = Green_Value.Value;
                    colorGrading.mixerBlueOutBlueIn.value = Blue_Value.Value;
                }



            }



        }
	}

}
