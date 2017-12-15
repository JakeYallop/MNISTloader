# MNISTloader
MNIST loader for C#

I've been working on some machine learning projects for a while now, and came across this awesome dataset called MNIST. It is a set of handwritten digits and labels for them, consisting of 60 000 training images and 10 000 test images. 

However, the data format is .gz , which requires it's own custom loader. As everyone using the database has to write their own, I figured I could share mine.

As an added bonus there is also a visualizer to make Bitmaps out of the raw sets of data for GUI purposes.
