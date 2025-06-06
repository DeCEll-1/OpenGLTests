DONE: add plane
    DONE: add cube
DONE: added basic opengl error logging to the logger function
DONE: added imgui
TODO: add instanced rendering
TODO: add text // as we have imgui, this isnt that important anymore
TODO: allow multiple lights
TODO: add a proper material with lightning and other features
    features include but are not limited to:
        wireframe
        texture
            offsets
            wrap types
        normal mapping
        flat shading
TODO: add gif rendering
TODO: PBR
    DONE: framebuffers
        DONE: implement post processing
        DONE: update PP FBO size when screen size changes
        DONE: implement disposal
        DONE: fix screenshot, its taking a blank screenshot currently
    DONE: cubemaps
    DONE: gamma correction
    TODO: HDR
    TODO: normal mapping
DONE: translate textures to init instead of initing on construction
DONE: add init and dispose functions to cubemap