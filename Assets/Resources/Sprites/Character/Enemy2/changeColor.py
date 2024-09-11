from PIL import Image

def hex_to_rgb(hex_color):
    hex_color = hex_color.lstrip('#')
    return tuple(int(hex_color[i:i+2], 16) for i in (0, 2, 4))

def change_pixel_color(image_path, main, main_light, main_dark, sub, sub_light, sub_dark, outline , output_path):
    _main = hex_to_rgb("#ff0002")
    _main_light = hex_to_rgb("#00bcff")
    _main_dark = hex_to_rgb("#5100ff")
    _sub = hex_to_rgb("#fff200")
    _sub_light = hex_to_rgb("#00ff1b")
    _sub_dark = hex_to_rgb("#d7ff00")
    _outline = hex_to_rgb("#000000")
    
    # Open the image
    img = Image.open(image_path).convert("RGBA")
    
    # Load the data of the image
    data = img.getdata()
    
    # Prepare the data for the new image
    new_data = []
    for item in data:
        # Check if this pixel matches the original color
        if item[:3] == _main:
            # Replace with the new color
            new_data.append(hex_to_rgb(main) + (item[3],))  # Preserve the alpha channel
        elif item[:3] == _main_light:
            new_data.append(hex_to_rgb(main_light) + (item[3],))  # Preserve the alpha channel
        elif item[:3] == _main_dark:
            new_data.append(hex_to_rgb(main_dark) + (item[3],))  # Preserve the alpha channel
        elif item[:3] == _sub:
            new_data.append(hex_to_rgb(sub) + (item[3],))  # Preserve the alpha channel
        elif item[:3] == _sub_light:
            new_data.append(hex_to_rgb(sub_light) + (item[3],))  # Preserve the alpha channel
        elif item[:3] == _sub_dark:
            new_data.append(hex_to_rgb(sub_dark) + (item[3],))  # Preserve the alpha channel
        elif item[:3] == _outline:
            new_data.append(hex_to_rgb(outline) + (item[3],))  # Preserve the alpha channel
        else:
            new_data.append(item)

    # Update image data
    img.putdata(new_data)
    
    # Create new directory if it doesn't exist
    import os
    os.makedirs(os.path.dirname(output_path), exist_ok=True)
    
    # Save the modified image
    img.save(output_path)
    print(f"Image saved to {output_path}")

# Example usage:
# Original color is "#FFFFFF" (white)
# New color is "#FF0000" (red)
def tomato(original_dir, png_file):
  change_pixel_color(os.path.join(original_dir, png_file), "#c0080a", "#e0080a", "#981012", "#529f36", "#7dc762", "#3f8227","#620a0c", original_dir+"/Tomato/"+png_file)
def corn(original_dir, png_file):
  change_pixel_color(os.path.join(original_dir, png_file), "#dfc226", "#ffe13f", "#c0a928", "#7bb31c", "#8ece20", "#669417","#948222", original_dir+"/Corn/"+png_file)
def broccoli(original_dir, png_file):
  change_pixel_color(os.path.join(original_dir, png_file), "#859c69", "#bdd2a2", "#66794e", "#c2d75a", "#e1f679", "#a2b733","#4b593a", original_dir+"/Broccoli/"+png_file)
def carrot(original_dir, png_file):
  change_pixel_color(os.path.join(original_dir, png_file), "#df7126", "#f68334", "#c5621f", "#6abe30", "#82d44a", "#4f8f23","#a95319", original_dir+"/Carrot/"+png_file)
def cauliflower(original_dir, png_file):
  change_pixel_color(os.path.join(original_dir, png_file), "#eddbd1", "#ffeee4", "#c9b7ad", "#868e4c", "#a9b170", "#707931","#a69891", original_dir+"/Cauliflower/"+png_file)
def eggplant(original_dir, png_file):
  change_pixel_color(os.path.join(original_dir, png_file), "#8c6d85", "#b791af", "#6b5566", "#13985e", "#21be7a", "#0a7446","#433640", original_dir+"/Eggplant/"+png_file)

import os

# Get all PNG files in the 'original' directory
original_dir = './'
png_files = [f for f in os.listdir(original_dir) if f.endswith('.png')]

# Apply all color transformations to each PNG file
for png_file in png_files:
    
    # Remove the file extension for the source parameter
    source_name = os.path.splitext(png_file)[0]
    
    # Apply all color transformations
    tomato(original_dir, png_file)
    corn(original_dir, png_file)
    broccoli(original_dir, png_file)
    carrot(original_dir, png_file)
    cauliflower(original_dir, png_file)
    eggplant(original_dir, png_file)

print("All color transformations completed for PNG files in the 'original' directory.")

