// Made by lovely Waveform
using UnityEngine.Rendering.PostProcessing;

namespace HutongGames.PlayMaker.Actions
{

	[ActionCategory("Post Processing V2")]
	public class GetAutoExposure : FsmStateAction
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

        //[ActionSection("Filtering")]
        //public FsmBool GetFiltering;
        [UIHint(UIHint.Variable)]
        public FsmVector2 FilteringValue;

        //[ActionSection("MinimumEV")]
        //public FsmBool GetMinimumEV;
        [UIHint(UIHint.Variable)]
        public FsmFloat MinimumEVValue;

        //[ActionSection("MaximumEV")]
        //public FsmBool GetMaximumEV;
        [UIHint(UIHint.Variable)]
        public FsmFloat MaximumEVValue;

        //[ActionSection("Adaption Type")]
        //public FsmBool getType;
        [UIHint(UIHint.Variable)]
        [ObjectType(typeof(EyeAdaptation))]
        public FsmEnum TypeValue;

        //[ActionSection("Speed Up")]
        //public FsmBool GetSpeedUp;
        [UIHint(UIHint.Variable)]
        public FsmFloat SpeedUpValue;

        //[ActionSection("Speed Down")]
        //public FsmBool GetSpeedDown;
        [UIHint(UIHint.Variable)]
        public FsmFloat SpeedDownValue;

        [ActionSection(" ")]
        public bool everyFrame;


        private PostProcessProfile convert;
        private PostProcessVolume convert2;

        public override void Reset()
        {
            Profile = null;
            convert = null;
            convert2 = null;
            /*
            GetEnable = false;
            EnableValue = false;

            GetFiltering = false;

            GetMinimumEV = false;

            GetMaximumEV = false;

            getType = false;

            GetSpeedUp = false;

            GetSpeedDown = false;
            */
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
                convert.TryGetSettings(out AutoExposure autoExposure);

                if (!EnableValue.IsNone)
                    EnableValue.Value = autoExposure.enabled.value;
                if (!FilteringValue.IsNone)
                    FilteringValue.Value = autoExposure.filtering.value;
                if (!MinimumEVValue.IsNone)
                    MinimumEVValue.Value = autoExposure.minLuminance.value;
                if (!MaximumEVValue.IsNone)
                    MaximumEVValue.Value = autoExposure.maxLuminance.value;
                if (!TypeValue.IsNone)
                    TypeValue.Value = autoExposure.eyeAdaptation.value;
                if (!SpeedUpValue.IsNone)
                    SpeedUpValue.Value = autoExposure.speedUp.value;
                if (!SpeedDownValue.IsNone)
                    SpeedDownValue.Value = autoExposure.speedDown.value;

            }



        }
	}

}
