import cv2
import time
import os
import asyncio
import winrt.windows.devices.enumeration as windows_devices

# DETECT CONNECTED CAMERAS AND SELECT WHICH ONE YOU WANT TO RECORD

async def get_camera_info():
    return await windows_devices.DeviceInformation.find_all_async(4)

connected_cameras = asyncio.run(get_camera_info())
names = [camera.name for camera in connected_cameras]

#for x in range(len(names)):
#    print("Connected camera with ID " + str(x) + " is -> " + names[x])

#camera_index = int(input('Which one you want to record? [answer with ID] \n'))
#camera_name = names[camera_index]

camera_name = names[1]

#print("Ok. You are recording from -> " + camera_name + '\n')
print("You are recording from -> " + camera_name + '\n')

# FRAME GRABBER FUNCTION WITH SOME TUNABLE PARAMETERS

def getFrames(folderName, imgExtension):

    frameRate=1
    width = 640
    height = 480
    desktopPath = os.path.join( os.path.join( os.environ['USERPROFILE']), 'Desktop')

    # full folder path
    #absPath = desktopPath + '/' + folderName + '_' + camera_name
    absPath = desktopPath + '/' + folderName

    # check the format of the filepath
    if not absPath[-1] == '/':
        absPath += '/'

    # check if the output folder already exists or create it
    if not os.path.exists(absPath):
        os.makedirs(absPath)
        print('Creating output folder...')
    else:
        print('Specified output folder already exists')

    # setting up the camera object 
    camera = cv2.VideoCapture()
    #camera.open(camera_index, cv2.CAP_DSHOW)
    camera.open(1, cv2.CAP_DSHOW)
    camera.set(cv2.CAP_PROP_FRAME_WIDTH, 1920)
    camera.set(cv2.CAP_PROP_FRAME_HEIGHT, 1080)
    print('Setting up the camera...')
    print('Resolution: ', width, ' x ', height)
    print('Frame rate: ', frameRate, 'fps')

    if camera.read():
        print('Starting camera recording...')
    else:
        print('WARNING: Cannot start camera recording!')

    i = 1
    while(camera.isOpened()):
        success, frame = camera.read()

        # if no frame is available, the reading stops
        if success == False:
            print("Cannot capture the frame")
            break

        # save frame in the specified folder
        filename = absPath + 'Frame' + str(i) + '_' + str(time.time()) + imgExtension
        cv2.imwrite(filename, frame)
        i += 1

        # set FPS
        time.sleep(0.25)

    camera.release()
    cv2.destroyAllWindows()


# MAIN (WIHT INPUT PARAMETERS)

if __name__ == '__main__':
    
    # setup
    # absPath = "C:/Users/komil/Desktop/provaCameraFrames/"
    absPath = "C:/Users/Abel/Desktop/Abel_Experiment_Frames/"
    imgExtension = ".jpg"
    # user input
    # frameRate = input('Specifiy the frame rate: \n')
    folderName = input('Specify the output folder path: \n')

    # start video recording
    getFrames(folderName, imgExtension)