shader_type canvas_item;

uniform float outline_px = 1.0;
uniform vec4 outline_color : source_color = vec4(0.0, 0.0, 0.0, 0.9);
uniform vec4 bg_color : source_color = vec4(0.0, 0.0, 0.0, 0.0);

void fragment() {
    vec4 base = texture(TEXTURE, UV);

    // If the base pixel is opaque, draw it as-is (no 'return' in Godot shaders)
    if (base.a > 0.0) {
        COLOR = base;
    } else {
        // Pixel step in UV units for 'outline_px' screen pixels
        vec2 texel = outline_px * TEXTURE_PIXEL_SIZE;

        // 8-neighbor alpha accumulation
        float a =
            texture(TEXTURE, UV + vec2( texel.x,  0.0)).a +
            texture(TEXTURE, UV + vec2(-texel.x,  0.0)).a +
            texture(TEXTURE, UV + vec2( 0.0,      texel.y)).a +
            texture(TEXTURE, UV + vec2( 0.0,     -texel.y)).a +
            texture(TEXTURE, UV + vec2( texel.x,  texel.y)).a +
            texture(TEXTURE, UV + vec2( texel.x, -texel.y)).a +
            texture(TEXTURE, UV + vec2(-texel.x,  texel.y)).a +
            texture(TEXTURE, UV + vec2(-texel.x, -texel.y)).a;

        if (a > 0.0) {
            COLOR = outline_color;   // draw outline where neighbor alpha exists
        } else {
            discard;                 // fully transparent
        }
    }
}
