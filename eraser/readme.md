# Eraser Test Outcome

Eraser will delete all images that don't have a running container with or without the scanner component.

## **Eraser Evaluation Checklist**

### **Setup**

- [x] **Set up a local Kubernetes cluster using `kind`.**
- [x] **Install the Eraser Helm chart onto the `kind` cluster.**

### **Test 1: Image with a Critical Vulnerability**

#### **1. Preparation:**
   - [x] Choose an image with a known critical vulnerability.
   - [x] Deploy the vulnerable image in a pod/container.
  
#### **2. Configuration:**
   - [x] Modify Eraser's configuration to run the cleanup every 1 minute.
   - [x] Ensure the scanner component is enabled with default settings.
    
#### **3. Execution:**
   - [x] Monitor the image and wait for the 1-minute interval.
  
#### **4. Validation:**
   - [x] Confirm the image with the vulnerability is deleted.

### **Test 2: Image with No Critical Vulnerabilities**

#### **1. Preparation:**
   - [x] Choose an image without any known critical vulnerabilities.
   - [x] Deploy the clean image in a pod/container.

#### **2. Configuration:**
   - [x] Modify Eraser's configuration to run the cleanup every 1 minute.
   - [x] Ensure the scanner component is enabled with default settings.
    
#### **3. Execution:**
   - [x] Monitor the image and wait for the 1-minute interval.

#### **4. Validation:**
   - [ ] Confirm the clean image is still present and not deleted.
     - This "FAILED", the image was deleted since it was not running.

### **Test 3: Both Types of Images, Scanner Disabled**

#### **1. Preparation:**
   - [x] Deploy both the vulnerable image and the clean image in separate pods/containers.

#### **2. Configuration:**
   - [x] Modify Eraser's configuration to run the cleanup every 1 minute.
   - [x] Disable the scanner component.

#### **3. Execution:**
   - [x] Monitor the images and wait for the 1-minute interval.

#### **4. Validation:**
   - [x] Confirm both images (vulnerable and clean) are deleted.

### **Cleanup**

- [x] **Delete all created pods and resources related to the tests.**
- [x] **Uninstall Eraser from the `kind` cluster.**
- [x] **Optionally, delete the `kind` cluster if you don't need it anymore.**
