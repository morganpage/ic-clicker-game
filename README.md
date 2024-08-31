# The IC GameKit Unity Demo Game - Clicker Game

## Introduction

This is a simple Clicker Game created in Unity to showcase the capabilities of the [IC GameKit](https://github.com/morganpage/ic-gamekit).

The game shows how you can integrate with Internet Identity to enable logging in and then how to interact with IC canisters to enable gameplay. The Unity Clicker Game interacts with the example clickergame.mo contract which in turn utilises the ic-gamekit to encorporate features such as achievements, player saves and game specific data.

## Installation

Clone this repo and open with Unity 6 preview or later. Running in the editor, the project will immediately try and open a browser window externally to log in to Internet Identity. On successful login it should then communicate back to the Unity editor via websockets to send the delegate chain which it uses to create a client capable of interacting with the IC as the logged in user.

## Internet Identity

To enable Unity to encorporate the Internet Identity login flow, a WebGLTemplate called DefaultWithIframe is used. The iframe is used to display a custom login page. The code for this can be found [here](https://github.com/morganpage/ic-gamekit/blob/main/src/unity-login/src/App.js). On successful login the delegate chain is passed to Unity either via websockets (Unity Editor) or by a postMessage (WebGL build).
