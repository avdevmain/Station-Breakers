// Made by lovely Waveform
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace HutongGames.PlayMaker.Actions
{

	[ActionCategory("Post Processing V2")]
	public class SetColorGradingTrackballs : FsmStateAction
	{
        [ActionSection("Profile")]
        [ObjectType(typeof(PostProcessProfile))]
        public FsmObject Profile;
        [ObjectType(typeof(PostProcessVolume))]
        public FsmObject Volume;

        [ActionSection("Enable")]
        public FsmBool SetEnable;
        public FsmBool EnableValue;

        [ActionSection("Trackballs Lift")]
        public FsmBool SetLift;
        public FsmColor LiftColor;
        /*
        [HasFloatSlider(0, 1)]
        public FsmFloat RedValue;
        [HasFloatSlider(0, 1)]
        public FsmFloat GreenValue;
        [HasFloatSlider(0, 1)]
        public FsmFloat BlueValue;
        */
        [HasFloatSlider(-1, 1)]
        public FsmFloat LiftSliderValue;

        [ActionSection("Trackballs Gamma")]
        public FsmBool SetGamma;
        public FsmColor GammaColor;
        /*
        [HasFloatSlider(0, 1)]
        public FsmFloat RedValue_;
        [HasFloatSlider(0, 1)]
        public FsmFloat GreenValue_;
        [HasFloatSlider(0, 1)]
        public FsmFloat BlueValue_;
        */
        [HasFloatSlider(-1, 1)]
        public FsmFloat GammaSliderValue;

        [ActionSection("Trackballs Gain")]
        public FsmBool SetGain;
        public FsmColor GainColor;
        /*
        [HasFloatSlider(0, 1)]
        public FsmFloat Red_Value;
        [HasFloatSlider(0, 1)]
        public FsmFloat Green_Value;
        [HasFloatSlider(0, 1)]
        public FsmFloat Blue_Value;
        */
        [HasFloatSlider(-1, 1)]
        public FsmFloat GainSliderValue;

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

            SetLift = false;
            SetGain = false;
            SetGamma = false;

            /*
            RedValue = 1;
            RedValue_ = 1;
            Red_Value = 1;

            GreenValue = 1;
            GreenValue_ = 1;
            Green_Value = 1;

            BlueValue = 1;
            BlueValue_ = 1;
            Blue_Value = 1;
            */

            LiftSliderValue = 0;
            GammaSliderValue = 0;
            GainSliderValue = 0;

            LiftColor = new Color(1,1,1,1);
            GammaColor = new Color(1, 1, 1, 1);
            GainColor = new Color(1, 1, 1, 1);

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
                if (SetLift.Value)
                {
                    //Vector4 ttop = new Vector4(RedValue.Value, GreenValue.Value, BlueValue.Value, SliderValue.Value);
                    Vector4 ttop = new Vector4(LiftColor.Value.r, LiftColor.Value.g, LiftColor.Value.b, LiftSliderValue.Value);
                    colorGrading.lift.value = ttop;
                }
                if (SetGamma.Value)
                {
                    //Vector4 ttop = new Vector4(RedValue_.Value, GreenValue_.Value, BlueValue_.Value, SliderValue_.Value);
                    Vector4 ttop = new Vector4(GammaColor.Value.r, GammaColor.Value.g, GammaColor.Value.b, GammaSliderValue.Value);
                    colorGrading.gamma.value = ttop;
                }
                if (SetGain.Value)
                {
                    //Vector4 ttop = new Vector4(Red_Value.Value,Green_Value.Value,Blue_Value.Value,Slider_Value.Value);
                    Vector4 ttop = new Vector4(GainColor.Value.r, GainColor.Value.g, GainColor.Value.b, GainSliderValue.Value);
                    colorGrading.gain.value = ttop;
                }



            }



        }
	}

}
