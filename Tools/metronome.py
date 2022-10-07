from datetime import datetime
from pynput import keyboard
import json
import time
from tkinter import *
from threading import Thread

# Tuning needed
# 120 - .25


# 140 - ABC
title = "abc"
bpm = 140

beat_time = 60 / bpm
currBeat = 0
notes = []
playing = True

prevBeat = -1
# Set up GUI
root = Tk()
root.geometry("1200x200")
canvas = Canvas(root, width=1200, height=200)
canvas.pack()
# Add rectangles to UI, list
a = canvas.create_rectangle(0, 0, 150, 150, fill='red')
canvas.move(a, 0, 0)
b = canvas.create_rectangle(0, 0, 150, 150, fill='red')
canvas.move(b, 250, 0)
c = canvas.create_rectangle(0, 0, 150, 150, fill='red')
canvas.move(c, 500, 0)
d = canvas.create_rectangle(0, 0, 150, 150, fill='red')
canvas.move(d, 750, 0)
rects = [a, b, c ,d]
index = 0

def runLoop():
    global currBeat, prevBeat, index, canvas
    print("Sleeping 1 sec to get timings right")
    time.sleep(1)
    # Get the start time
    orig_time = datetime.now()
    # We pretty much always want to be running this, until we get a specific key for esc
    while(playing):
        curr_time = datetime.now()
        diff = curr_time - orig_time
        # diff.total_seconds
        num = diff.total_seconds() / beat_time
        currBeat = roundToSixteenth(num)
        if int(currBeat) == currBeat and int(currBeat) > prevBeat:
            # Terminal print as well, to help
            print("\n\n\n\n" + str(currBeat) + "   " + str(num))
            # Change the rectangle colors
            canvas.itemconfig(rects[index], fill='green')
            canvas.itemconfig(rects[(index + 3) % 4], fill='red')
            index = (index + 1) % 4

            prevBeat = currBeat

# Helper method to access 
def roundToSixteenth(num):
    numint = int(num)
    diff = num - numint
    choices = [0, .125, .25, .375, .5, .625, .75, .875]
    if 0 <= diff and diff < .25:
        return numint
    if .25 <= diff and diff < .5:
        return numint + .25 
    if .5 <= diff and diff < .75:
        return numint + .5
    if .75 <= diff and diff < 1:
        return numint + .75

def on_press(key):
    global playing
    try:
        print(currBeat - 0)
        notes.append(currBeat - 0)
        if key == keyboard.Key.esc:
            print("escaping")
            playing = False
            canvas.delete()
            print(notes)
            with open('{title}_notes.json'.format(title = title), 'w') as fp:
                json.dump(notes, fp, indent = 4)
            root.destroy()

    except AttributeError:
        if key == keyboard.Key.esc:
            print("escaping")
            playing = False
        print("idk")

listener = keyboard.Listener(
   on_press=on_press,
    )
listener.start()
thread = Thread(target=runLoop)
thread.start()
root.mainloop()

while thread.is_alive:
    continue

print(notes)
with open('{title}_notes.json'.format(title = title), 'w') as fp:
    json.dump(notes, fp, indent = 4)
