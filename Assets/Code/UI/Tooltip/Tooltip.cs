using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public class Tooltip : MonoBehaviour
    {
        [SerializeField] private Image background;
        [SerializeField] private TextMeshProUGUI message;
        [SerializeField] private float visibleDuration = 2.0f;
        [SerializeField] private float fadeDuration = 1.0f;

        private IEnumerator hidingCoroutine;
        
        public void Show(string info, Vector3 position)
        {
            message.SetText(info);
            transform.SetPositionAndRotation(position, Quaternion.identity);

            var backgroundColour = background.color;
            backgroundColour.a = 1f;
            background.color = backgroundColour;
            
            var messageColour = message.color;
            messageColour.a = 1f;
            message.color = messageColour;
            
            gameObject.SetActive(true);
            
            if (hidingCoroutine != null) StopCoroutine(hidingCoroutine);
            hidingCoroutine = HideCoroutine();
            StartCoroutine(hidingCoroutine);
        }

        public void HideInstant()
        {
            if (hidingCoroutine != null) StopCoroutine(hidingCoroutine);
            hidingCoroutine = null;
            gameObject.SetActive(false);
        }

        private IEnumerator HideCoroutine()
        {
            var elapsed = 0f;

            var startBackgroundColor = background.color;
            var startMessageColor = message.color;
            
            yield return new WaitForSeconds(visibleDuration - fadeDuration);

            while (elapsed < fadeDuration)
            {
                var t = elapsed / fadeDuration;

                var newBackgroundColor = startBackgroundColor;
                newBackgroundColor.a = Mathf.Lerp(1f, 0f, t);
                background.color = newBackgroundColor;

                var newMessageColor = startMessageColor;
                newMessageColor.a = Mathf.Lerp(1f, 0f, t);
                message.color = newMessageColor;

                elapsed += Time.deltaTime;
                yield return null;
            }

            var finalBackgroundColor = background.color;
            finalBackgroundColor.a = 0f;
            background.color = finalBackgroundColor;

            var finalMessageColor = message.color;
            finalMessageColor.a = 0f;
            message.color = finalMessageColor;

            gameObject.SetActive(false);
            hidingCoroutine = null;
        }
    }
}