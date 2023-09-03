using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

#pragma warning disable IDE1006
// ! This name must match the name in the main class file.
partial class IDVerification
{
#pragma warning disable 414, IDE0051
    private readonly string TwitchHelpMessage = @"Use '!{0} accept' | '!{0} deny' to press the green or red button.";
#pragma warning restore 414, IDE1006

    private IEnumerator ProcessTwitchCommand(string command) {
        
        command = command.Trim().ToUpperInvariant();

        if (command == string.Empty)
            yield return "sendtochaterror That's an empty command...";

        yield return null;

        if (command == "ACCEPT")
            _acceptButton.OnInteract();
        else if (command == "DENY")
            _denyButton.OnInteract();
        else
        {
            yield return "sendtochaterror Invalid command.";
            yield break;
        }
    }

    private IEnumerator TwitchHandleForcedSolve() {
        for (int i = 0; i < CorrectAnswers.Count; i++)
        {
            if (CorrectAnswers[i] == true)
                _acceptButton.OnInteract();
            else
                _denyButton.OnInteract();
            yield return new WaitForSeconds(.1f);
        }
    }
#pragma warning restore IDE0051
}