import cv2
import time
import os

# FRAME GRABBER FUNCTION WITH SOME TUNABLE PARAMETERS

def getFrames(folderName, imgExtension):

    frameRate = 1
    # width = 640
    # height = 480
    width = 1920
    height = 1080

    desktopPath = os.path.join(os.path.join(os.path.expanduser('~')), 'Desktop')

    absPath = os.path.join(desktopPath, folderName)

    if not os.path.exists(absPath):
        os.makedirs(absPath)
        print('Creating output folder...')
    else:
        print('Specified output folder already exists')

    camera = cv2.VideoCapture(0)  # Adjust the camera index as needed
    camera.set(cv2.CAP_PROP_FRAME_WIDTH, width) #
    camera.set(cv2.CAP_PROP_FRAME_HEIGHT, height) # Adjust the camera index as needed
    print('Setting up the camera...')
    print('Resolution: ', width, ' x ', height)
    print('Frame rate: ', frameRate, 'fps')

    if camera.isOpened():
        print('Starting camera recording...')
    else:
        print('WARNING: Cannot start camera recording!')

    i = 1
    while camera.isOpened():
        success, frame = camera.read()

        if not success:
            print("Cannot capture the frame")
            break

        filename = os.path.join(absPath, f'Frame{i}_{time.time()}{imgExtension}')
        cv2.imwrite(filename, frame)
        print(filename + '\n')
        i += 1

        time.sleep(0.25)

    camera.release()
    cv2.destroyAllWindows()

# MAIN FUNCTION (WITH INPUT PARAMETERS)

if __name__ == '__main__':
    
    absPath = os.path.join(os.path.expanduser('~'), 'Desktop', 'Experiment_Frames')
    imgExtension = ".jpg"
    folderName = input('Specify the output folder name: \n')

    getFrames(folderName, imgExtension)
