using System.Collections.Generic;
using UnityEngine;

namespace Mithmarie
{
    public class CloneSelectionPreview : StateBehaviour
    {
        public void Show(HashSet<Vector3Int> blocks)
        {
            // generate mesh.

            // setu[
        }

        public void UpdatePreview(HoverInformation hover)
        {
            if (hover == null)
            {
                Hide();
                return;
            }
            
            /*facePreview.gameObject.SetActive(hover.hasHit);
            cubePreview.gameObject.SetActive(!hover.hasHit);

            cubePreview.position = hover.pos;
            if (!hover.hasHit)
                return;

            facePreview.position = hover.pos;
            facePreview.forward = hover.normal;*/
        }

        public void Hide()
        {

        }
    }
}
