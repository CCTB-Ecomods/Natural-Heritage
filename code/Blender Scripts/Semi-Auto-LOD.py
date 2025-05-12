import bpy

MatList = []

#Select the Objects and collect their Materials all_objects[1:]
for obj in bpy.data.collections['Collection'].all_objects:
    print(obj.name)
    obj.select_set(True)
    MatList.extend(obj.data.materials)

#cleanup Matterials  
MatList = list(set(MatList))
obj = bpy.context.active_object
temp = []
temp.extend(obj.data.materials)
MatList = [x for x in MatList if x not in temp]
print(MatList)

#Add Materials to merge Object
for mat in MatList:
    obj.data.materials.append(mat)

#Union
bpy.ops.object.booltool_auto_union()

#Add Decimate and rename
C = bpy.context
src_obj = bpy.context.active_object
src_obj.name = "Content_LOD0"
src_obj.data.name = "Content_LOD0"
bpy.ops.object.modifier_add(type='DECIMATE')

#Duplicate and name    
for i in range(1,3):
    new_obj = src_obj.copy()
    new_obj.data = src_obj.data.copy()
    new_obj.animation_data_clear()
    C.collection.objects.link(new_obj)
    new_obj.name = "Content_LOD%d"%(i)
    new_obj.data.name = "Content_LOD%d"%(i)

