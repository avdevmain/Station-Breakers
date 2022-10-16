// Made by lovely Waveform
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace HutongGames.PlayMaker.Actions
{

	[ActionCategory("Post Processing V2")]
	public class GetColorGradingTrackballs : FsmStateAction
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

        [ActionSection("Trackballs Lift")]
        public FsmBool GetLift;
        [UIHint(UIHint.Variable)]
        public FsmColor LiftColor;
        /*
        [UIHint(UIHint.Variable)]
        public FsmFloat RedValue;
        [UIHint(UIHint.Variable)]
        public FsmFloat GreenValue;
        [UIHint(UIHint.Variable)]
        public FsmFloat BlueValue;
        */
        [UIHint(UIHint.Variable)]
        public FsmFloat LiftSliderValue;

        [ActionSection("Trackballs Gamma")]
        public FsmBool GetGamma;
        [UIHint(UIHint.Variable)]
        public FsmColor GammaColor;
        /*
        [UIHint(UIHint.Variable)]
        public FsmFloat RedValue_;
        [UIHint(UIHint.Variable)]
        public FsmFloat GreenValue_;
        [UIHint(UIHint.Variable)]
        public FsmFloat BlueValue_;
        */
        [UIHint(UIHint.Variable)]
        public FsmFloat GammaSliderValue;

        [ActionSection("Trackballs Gain")]
        public FsmBool GetGain;
        [UIHint(UIHint.Variable)]
        public FsmColor GainColor;
        /*
        [UIHint(UIHint.Variable)]
        public FsmFloat Red_Value;
        [UIHint(UIHint.Variable)]
        public FsmFloat Green_Value;
        [UIHint(UIHint.Variable)]
        public FsmFloat Blue_Value;
        */
        [UIHint(UIHint.Variable)]
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

            GetEnable = false;
            EnableValue = false;

            GetLift = false;
            GetGain = false;
            GetGamma = false;

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
                    EnableValue.Value = colorGrading.enabled.value;
                if (GetLift.Value)
                {
                    Vector4 go = colorGrading.lift.value;
                    LiftColor.Value = new Color(go.x, go.y, go.z);
                    /*
                    RedValue.Value = go.x;
                    GreenValue.Value = go.y;
                    BlueValue.Value = go.z;
                    */
                    LiftSliderValue.Value = go.w;
                }
                if (GetGamma.Value)
                {
                    Vector4 go = colorGrading.gamma.value;
                    GammaColor.Value = new Color(go.x, go.y, go.z);
                    /*
                    RedValue_.Value = go.x;
                    GreenValue_.Value = go.y;
                    BlueValue_.Value = go.z;
                    */
                    GammaSliderValue.Value = go.w;
                }
                if (GetGain.Value)
                {
                    Vector4 go = colorGrading.lift.value;
                    GainColor.Value = new Color(go.x, go.y, go.z);
                    /*
                    Red_Value.Value = go.x;
                    Green_Value.Value = go.y;
                    Blue_Value.Value = go.z;
                    */
                    GainSliderValue.Value = go.w;
                }



            }



        }
	}

}
