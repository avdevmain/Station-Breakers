// Made by lovely Waveform

using UnityEngine.Rendering.PostProcessing;

namespace HutongGames.PlayMaker.Actions
{

	[ActionCategory("Post Processing V2")]
	public class GetActiveColorGradingTrackballs : FsmStateAction
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

        //[ActionSection("Trackballs Lift")]
        //public FsmBool GetLift;
        [UIHint(UIHint.Variable)]
        public FsmBool LiftColor;

        //[ActionSection("Trackballs Gamma")]
        //public FsmBool GetGamma;
        [UIHint(UIHint.Variable)]
        public FsmBool GammaColor;


        //[ActionSection("Trackballs Gain")]
        //public FsmBool GetGain;
        [UIHint(UIHint.Variable)]
        public FsmBool GainColor;


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
            //EnableValue = false;

            //GetLift = false;
            //GetGain = false;
            //GetGamma = false;

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
                if (!EnableValue.IsNone)
                    EnableValue.Value = colorGrading.active;
                if (!LiftColor.IsNone)
                {
                    LiftColor.Value = colorGrading.lift.overrideState;
                }
                if (!GammaColor.IsNone)
                {
                    GammaColor.Value = colorGrading.gamma.overrideState;
                }
                if (!GainColor.IsNone)
                {
                    GainColor.Value = colorGrading.gain.overrideState;
                }



            }



        }
	}

}
