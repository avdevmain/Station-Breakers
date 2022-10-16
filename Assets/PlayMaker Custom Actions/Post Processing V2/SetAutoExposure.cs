// Made by lovely Waveform
using UnityEngine.Rendering.PostProcessing;

namespace HutongGames.PlayMaker.Actions
{

	[ActionCategory("Post Processing V2")]
	public class SetAutoExposure : FsmStateAction
	{
        [ActionSection("Profile")]
        [ObjectType(typeof(PostProcessProfile))]
        public FsmObject Profile;
        [ObjectType(typeof(PostProcessVolume))]
        public FsmObject Volume;

        [ActionSection("Enable")]
        public FsmBool SetEnable;
        public FsmBool EnableValue;

        [ActionSection("Filtering")]
        public FsmBool SetFiltering;
        public FsmVector2 FilteringValue;

        [ActionSection("MinimumEV")]
        public FsmBool SetMinimumEV;
        [HasFloatSlider(-9,9)]
        public FsmFloat MinimumEVValue;

        [ActionSection("MaximumEV")]
        public FsmBool SetMaximumEV;
        [HasFloatSlider(-9, 9)]
        public FsmFloat MaximumEVValue;
        /*
        [ActionSection("Compensation")]
        public FsmBool SetCompensation;
        public FsmFloat CompensationValue;
        */
        [ActionSection("Adaption Type")]
        public FsmBool SetType;
        [ObjectType(typeof(EyeAdaptation))]
        public FsmEnum TypeValue;

        [ActionSection("Speed Up")]
        public FsmBool SetSpeedUp;
        public FsmFloat SpeedUpValue;

        [ActionSection("Speed Down")]
        public FsmBool SetSpeedDown;
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

            SetEnable = false;
            EnableValue = false;

            SetFiltering = false;
            FilteringValue = default;

            SetMinimumEV = false;
            MinimumEVValue = 0;

            SetMaximumEV = false;
            MaximumEVValue = 0;
            /*
            SetCompensation = false;
            CompensationValue = 1;
            */
            SetType = false;
            TypeValue = EyeAdaptation.Progressive;

            SetSpeedUp = false;
            SpeedUpValue = 2;

            SetSpeedDown = false;
            SpeedDownValue = 1;

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
                convert.TryGetSettings(out AutoExposure autoExposure);

                if (SetEnable.Value)
                    autoExposure.enabled.value = EnableValue.Value;
                if (SetFiltering.Value)
                    autoExposure.filtering.value = FilteringValue.Value;
                if (SetMinimumEV.Value)
                    autoExposure.minLuminance.value = MinimumEVValue.Value;
                if (SetMaximumEV.Value)
                    autoExposure.maxLuminance.value = MaximumEVValue.Value;
                if (SetType.Value)
                    autoExposure.eyeAdaptation.value = (EyeAdaptation)TypeValue.Value;
                if (SetSpeedUp.Value)
                    autoExposure.speedUp.value = SpeedUpValue.Value;
                if (SetSpeedDown.Value)
                    autoExposure.speedDown.value = SpeedDownValue.Value;

            }



        }
	}

}
