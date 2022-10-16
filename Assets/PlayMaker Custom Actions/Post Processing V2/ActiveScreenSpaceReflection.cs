// Made by lovely Waveform
using UnityEngine.Rendering.PostProcessing;

namespace HutongGames.PlayMaker.Actions
{

	[ActionCategory("Post Processing V2")]
	public class ActiveScreenSpaceReflections : FsmStateAction
	{
        [ActionSection("Profile")]
        [ObjectType(typeof(PostProcessProfile))]
        public FsmObject Profile;
        [ObjectType(typeof(PostProcessVolume))]
        public FsmObject Volume;

        //[ActionSection("Enable")]
        public FsmBool ActiveEnable;
        //public FsmBool EnableValue;

        //[ActionSection("Preset")]
        public FsmBool ActivePreset;
        //public FsmBool PresetValue;

        //[ActionSection("Max March Distance")]
        public FsmBool ActiveMaxMarchDistance;
        //public FsmBool MaxMarchDistanceValue;

        //[ActionSection("Distance Fade")]
        public FsmBool ActiveDistanceFade;
        //public FsmBool DistanceFadeValue;

        //[ActionSection("Vignette")]
        public FsmBool ActiveVignette;
        //public FsmBool VignetteValue;

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

            ActivePreset = new FsmBool { UseVariable = true };

            ActiveMaxMarchDistance = new FsmBool { UseVariable = true };

            ActiveDistanceFade = new FsmBool { UseVariable = true };

            ActiveVignette = new FsmBool { UseVariable = true };

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
                convert.TryGetSettings(out ScreenSpaceReflections screenSpaceReflections);

                if (!ActiveEnable.IsNone)
                    screenSpaceReflections.active = ActiveEnable.Value;
                if (!ActivePreset.IsNone)
                    screenSpaceReflections.preset.overrideState = ActivePreset.Value;
                if (!ActiveMaxMarchDistance.IsNone)
                    screenSpaceReflections.maximumMarchDistance.overrideState = ActiveMaxMarchDistance.Value;
                if (!ActiveDistanceFade.IsNone)
                    screenSpaceReflections.distanceFade.overrideState = ActiveDistanceFade.Value;
                if (!ActiveVignette.IsNone)
                    screenSpaceReflections.vignette.overrideState = ActiveVignette.Value;

            }



        }
	}

}
